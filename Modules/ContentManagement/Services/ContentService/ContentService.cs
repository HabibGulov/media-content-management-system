public class ContentService : IContentService
{
    private readonly IUnitOfWork<Content> unitOfWork;
    private readonly IFileService fileService;
    private readonly IWebHostEnvironment hostEnvironment;
    private readonly IUserService userService;

    public ContentService(IUnitOfWork<Content> unitOfWork, IFileService fileService, IWebHostEnvironment hostEnvironment, IUserService userService)
    {
        this.userService = userService;
        this.unitOfWork = unitOfWork;
        this.fileService = fileService;
        this.hostEnvironment = hostEnvironment;
    }

    public async Task<Result<ContentReadInfo>> GetContentById(Guid contentId)
    {
        var findRepository = unitOfWork.FindRepository;

        Content? content = await findRepository.GetByIdAsync(contentId);
        if (content == null) return Result<ContentReadInfo>.Failure(Error.NotFound());

        return Result<ContentReadInfo>.Success(new ContentReadInfo
        {
            Id = content.Id,
            BaseInfo = new ContentBaseInfo(
                content.Title,
                content.Description,
                content.Url,
                content.IsFree,
                content.Price.GetValueOrDefault(),
                content.UserId,
                content.ContentType
            ),
        });
    }

    public async Task<Result<bool>> CreateContent(ContentCreateInfo contentCreateInfo, Guid userId)
    {

        var user = await userService.GetUserById(userId);
        if (user == null || user.Value.Role == null || user.Value.Role != "Admin")
        {
            return Result<bool>.Failure(Error.BadRequest("You must be an admin to create content."));
        }


        var contentExists = await unitOfWork.FindRepository
            .FindAsync(x => x.Url.ToLower() == contentCreateInfo.BaseInfo.Url.ToLower());

        if (contentExists == null) return Result<bool>.Failure(Error.AlreadyExist());

        string folder = contentCreateInfo.BaseInfo.ContentType switch
        {
            ContentType.Video => MediaFolders.Videos,
            ContentType.Image => MediaFolders.Images,
            ContentType.File => MediaFolders.Documents,
            _ => throw new InvalidOperationException("Unsupported content type")
        };

        string fileName = await fileService.CreateFile(contentCreateInfo.File, folder);

        var newContent = new Content
        {
            Title = contentCreateInfo.BaseInfo.Title,
            Description = contentCreateInfo.BaseInfo.Description,
            Url = fileName,
            IsFree = contentCreateInfo.BaseInfo.IsFree,
            Price = contentCreateInfo.BaseInfo.Price,
            UserId = contentCreateInfo.BaseInfo.UserId,
            ContentType = contentCreateInfo.BaseInfo.ContentType
        };

        await unitOfWork.AddRepository.AddAsync(newContent);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> UpdateContent(Guid contentId, ContentUpdateInfo contentUpdateInfo, Guid userId)
    {
        var contentRepository = unitOfWork.FindRepository;
        var updateRepository = unitOfWork.UpdateRepository;

        var user = await userService.GetUserById(userId);
        if (user == null || user.Value.Role == null || user.Value.Role != "Admin")
        {
            return Result<bool>.Failure(Error.BadRequest("You must be an admin to create content."));
        }

        Content? existingContent = await contentRepository.GetByIdAsync(contentId);
        if (existingContent == null) return Result<bool>.Failure(Error.NotFound());

        existingContent.Title = contentUpdateInfo.BaseInfo.Title;
        existingContent.Description = contentUpdateInfo.BaseInfo.Description;
        existingContent.Url = contentUpdateInfo.BaseInfo.Url;
        existingContent.IsFree = contentUpdateInfo.BaseInfo.IsFree;
        existingContent.Price = contentUpdateInfo.BaseInfo.Price;

        if (contentUpdateInfo.File != null)
        {
            fileService.DeleteFile(existingContent.Url, MediaFolders.Images);
            string newFileName = await fileService.CreateFile(contentUpdateInfo.File, existingContent.ContentType switch
            {
                ContentType.Video => MediaFolders.Videos,
                ContentType.Image => MediaFolders.Images,
                ContentType.File => MediaFolders.Documents,
                _ => throw new InvalidOperationException("Unsupported content type")
            });
            existingContent.Url = newFileName;
        }

        updateRepository.Update(existingContent);

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public async Task<Result<bool>> DeleteContent(Guid contentId, Guid userId)
    {
        var contentRepository = unitOfWork.FindRepository;
        var deleteRepository = unitOfWork.DeleteRepository;

        var user = await userService.GetUserById(userId);
        if (user == null || user.Value.Role == null || user.Value.Role != "Admin")
        {
            return Result<bool>.Failure(Error.BadRequest("You must be an admin to create content."));
        }

        Content? content = await contentRepository.GetByIdAsync(contentId);
        if (content == null) return Result<bool>.Failure(Error.NotFound());

        deleteRepository.Delete(content);
        fileService.DeleteFile(content.Url, content.ContentType switch
        {
            ContentType.Video => MediaFolders.Videos,
            ContentType.Image => MediaFolders.Images,
            ContentType.File => MediaFolders.Documents,
            _ => throw new InvalidOperationException("Unsupported content type")
        });

        return await unitOfWork.Complete() != 0
            ? Result<bool>.Success(true)
            : Result<bool>.Failure(Error.InternalServerError());
    }

    public Task<string> GetContentUrl(Guid contentId)
    {
        return Task.FromResult(Path.Combine(hostEnvironment.WebRootPath, contentId.ToString()));
    }

    public async Task<Result<List<ContentReadInfo>>> GetAllContents(ContentFilter filter)
    {
        var findRepository = unitOfWork.FindRepository;

        var contents = await findRepository.FindAsync(x =>
            (filter.UserId == null)        
        );

        if (contents == null || contents.Count() == 0)
        {
            return Result<List<ContentReadInfo>>.Failure(Error.NotFound());
        }

        var contentReadInfos = contents.Select(content => new ContentReadInfo
        {
            Id = content.Id,
            BaseInfo = new ContentBaseInfo(
                content.Title,
                content.Description,
                content.Url,
                content.IsFree,
                content.Price.GetValueOrDefault(),
                content.UserId,
                content.ContentType
            )
        }).ToList();

        return Result<List<ContentReadInfo>>.Success(contentReadInfos);
    }
}