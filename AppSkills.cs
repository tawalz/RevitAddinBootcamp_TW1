using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace RevitAddinBootcamp_TW1
{
    public class AppSkills : IExternalApplication
    {
        public Result OnStartup(UIControlledApplication app)
        {
            // 1. Create tab
            string tabName = "My Tab";
            app.CreateRibbonTab(tabName);

            // 1b. Create tab and panel - safer version
            try
            {
                app.CreateRibbonTab(tabName);
            }
            catch (Exception error)   
            {
                Debug.Print("Tab already exists.  Using existing panel.");
                Debug.Print(error.Message);
            }

            // 2. Create panel
            string panelName1 = "Test Panel";
            string panelName2 = "Test Panel 2";
            //string panelName3 = "Test Panel 3";

            RibbonPanel panel = app.CreateRibbonPanel(tabName, panelName1); // these process puts it in the new tab we created
            RibbonPanel panel2 = app.CreateRibbonPanel(panelName2); // these process puts it in the revit add-in tab
            //RibbonPanel panel3 = app.CreateRibbonPanel("Architecture",panelName3);

            // 2a. Get Existing Panel
            List<RibbonPanel> panelList = app.GetRibbonPanels();
            List<RibbonPanel> panelList2 = app.GetRibbonPanels(tabName);

            // 2b. Create/Get panel method - safe method
            RibbonPanel panel4 = CreateGetPanel(app, tabName, panelName1);

            // 3. Create button data
            PushButtonData buttonData1 = new PushButtonData(
                "button1",
                "Skill 01",
                Assembly.GetExecutingAssembly().Location,
                "RevitAddinBootcamp_TW1.cmdSkills01");

            PushButtonData buttonData2 = new PushButtonData(
               "button2",
               "Button\rSkill 02", //by adding \r this breaks the sentence into two lines
               Assembly.GetExecutingAssembly().Location,
               "RevitAddinBootcamp_TW1.cmdSkills02");

            // 4. Add tooltips
            buttonData1.ToolTip = "This is what Command 1 does";
            buttonData2.ToolTip = "This is what Command 2 does";

            // 5. add images
            buttonData1.LargeImage = ConvertToImageSource(Properties.Resources.Green_32);
            buttonData1.Image = ConvertToImageSource(Properties.Resources.Green_16);
            buttonData2.LargeImage = ConvertToImageSource(Properties.Resources.Blue_32);
            buttonData2.Image = ConvertToImageSource(Properties.Resources.Blue_16);

            // 6. Create Buttons
            //PushButton button1 = panel.AddItem(buttonData1) as PushButton;
            //PushButton button2 = panel.AddItem(buttonData2) as PushButton;

            // 7. Add stackable button
            //panel.AddStackedItems(buttonData1, buttonData2 );

            // 8. Add split button
            //SplitButtonData splitButtonData = new SplitButtonData("splitButton", "Split/rButton");
            //SplitButton splitButton = panel.AddItem(splitButtonData)as SplitButton;
            //splitButton.AddPushButton(buttonData1);
            //splitButton.AddPushButton(buttonData2);

            //9. Add pull down button
            PulldownButtonData pulldownButtonData = new PulldownButtonData("pulldownButton", "Pulldowm\rButton");
            pulldownButtonData.LargeImage = ConvertToImageSource(Properties.Resources.Green_32);

            PulldownButton pulldownButton=panel.AddItem(pulldownButtonData) as PulldownButton;
            pulldownButton.AddPushButton(buttonData1);
            pulldownButton.AddPushButton(buttonData2);

            // 10. other items
            panel.AddSeparator();
            panel.AddSlideOut();


            return Result.Succeeded;
        }

        private RibbonPanel CreateGetPanel(UIControlledApplication app, string tabName, string panelName1)
        {
            //look for panel in tab
            foreach (RibbonPanel panel in app.GetRibbonPanels(tabName))
            {
                if (panel.Name == panelName1)
                {
                    return panel;
                }
            }

            // panel not found, create it
            RibbonPanel returnPanel = app.CreateRibbonPanel(tabName,panelName1);
            return returnPanel;

            /* panel not found can also be done as follows
                return app.CreateRibbonPanel(tabName, panelName1);
             */ 
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public BitmapImage ConvertToImageSource(byte[] imageData)
        {
            using (MemoryStream mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.StreamSource = mem;
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.EndInit();

                return bmi;
            }
        }


    }
}
