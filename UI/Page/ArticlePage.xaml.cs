using System.Windows;
using System.Windows.Controls;
using ChicTrash.UI.Component;
using ChicTrash.UI.Windows;

namespace ChicTrash.UI.Page;

public partial class ArticlePage : System.Windows.Controls.Page
{
    
    private DatabaseService _dbService;
    private List<Article> _articles;
    public ArticlePage()
    {
        InitializeComponent();
        _dbService = Application.Current.Windows.OfType<Home>().FirstOrDefault()?._dbService;
    }

    private void ArticleWrapPanel_OnLoaded(object sender, RoutedEventArgs e)
    {
        _articles = _dbService.GetArticles();
        foreach (var article in _articles)
        {
            var Article = new ItemCard
            {
                Margin = new Thickness(10)
                
            };
            Article.productName.Text = article.Title;
            Article.productPrice.Text = "";
            ArticleWrapPanel.Children.Add(Article);
            
            Article.MouseLeftButtonUp += (sender, e) =>
            {
                Application.Current.Windows.OfType<Home>().FirstOrDefault().ContentFrame.Navigate(new ArticleViewPage(article));
            };
        }
        
    }
} 