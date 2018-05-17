using Altaaref.Models;
using Altaaref.UserControls.AccordionView;
using Altaaref.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Altaaref.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SelectCoursesForRegisterationPage : ContentPage
	{
        public SelectCoursesForRegisterationPage(List<Faculty> FacultiesSelectedList)
		{
            var syncTask = new Task(() =>
                BindingContext = new SelectCoursesForRegisterationViewModel(new PageService(), FacultiesSelectedList)
            );

            syncTask.RunSynchronously();

            var syncSecond = new Task(() =>
            {
                this.Title = "Accordion";

                var toolbarItem = new ToolbarItem();
                toolbarItem.Text = "Done";
                toolbarItem.SetBinding(ToolbarItem.CommandProperty, "DoneCommand");
                this.ToolbarItems.Add(toolbarItem);

                var template = new DataTemplate(typeof(DefaultTemplate));

                var bind = new Binding(path: "BindingContext", source: this);

                template.SetBinding(DefaultTemplate.ParentContextProperty, bind);

                var view = new AccordionView(template);
                view.SetBinding(AccordionView.ItemsSourceProperty, "List");
                view.Template.SetBinding(AccordionSectionView.TextProperty, "Text");
                view.Template.SetBinding(AccordionSectionView.ItemsSourceProperty, "List");


                view.BindingContext = (BindingContext as SelectCoursesForRegisterationViewModel).ViewModel;

                this.Content = view;
            });
            
            syncSecond.RunSynchronously();
        }
    }
}