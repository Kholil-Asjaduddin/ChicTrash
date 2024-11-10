namespace ChicTrash;

public class Article
{
    private int _articleId;
    private string _title;
    private string _content;

    public string Title
    {
        get => _title;
        set => _title = value;
    }

    public string Content
    {
        get => _content;
        set => _content = value;
    }

    public int ArticleId
    {
        get => _articleId;
        set => _articleId = value;
    }
}