using System.Windows.Controls;

namespace ChicTrash.UI.Page;

public partial class ArticleViewPage : System.Windows.Controls.Page
{
    public ArticleViewPage(Article article)
    {
        InitializeComponent();
        ArticleDescriptionTxtBlock.Text = article.Content;
        ArticleTitleTxtBlock.Text = article.Title;
    }
}