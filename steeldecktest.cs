using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using RevitAddinBootcamp_TW1.Common;


namespace RevitAddinBootcamp_TW1
{
    [Transaction(TransactionMode.Manual)]
    public class steeldecktest : IExternalCommand    
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)        
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // 1. Filter for all floor elements in the document
            FilteredElementCollector flrColl = new FilteredElementCollector(doc);
            flrColl.OfClass(typeof(FloorType));


            using (Transaction t = new Transaction(doc, "Identify Steel Deck Profiles"))
            {
                t.Start();


                // 2. Loop through each floor
                int counter = 0;


                               
                
                foreach (FloorType flrType in flrColl)
                 {
                                

                     // 3. Get Floor types and deck profile
                        
                    if (flrType != null)
                    {

                        



                        CompoundStructure flrStruc = flrType.GetCompoundStructure();
                        if (flrStruc != null)
                        {
                            
                            List<CompoundStructureLayer> flrLayers = flrStruc.GetLayers().ToList();
                            
                            double slabTk = flrStruc.GetWidth();



                            if (flrLayers != null)
                            {
                                
                                foreach (CompoundStructureLayer layer in flrLayers)
                                {
                                    


                                    FamilySymbol deckPro = doc.GetElement(layer.DeckProfileId) as FamilySymbol;
                                    if (deckPro != null)
                                    {
                                        string proName = deckPro.Name;

                                        Parameter parameter = flrType.LookupParameter("Deck Type");
                                        //replace comments above with correct shared parameter name


                                        {
                                            parameter.Set(proName);


                                        }



                                        List<Parameter> deckTk = deckPro.GetParameters("hr").ToList();
                                        if (deckTk != null)
                                        {
                                            foreach (Parameter param in deckTk)
                                            {
                                                double deckThick = param.AsDouble();
                                                if (deckThick != 0)
                                                {
                                                    double slabThick = slabTk - deckThick;

                                                    Parameter parameter3 = flrType.LookupParameter("Deck Height");
                                                    {
                                                        parameter3.Set(deckThick);
                                                    
                                                    }
                                                    
                                                    Parameter parameter2 = flrType.LookupParameter("Floor Slab Thickness");
                                                    {
                                                        parameter2.Set(slabThick);
                                                        counter++;
                                                    }
                                                }
                                            }


                                        }

                                    }


                                    

                                }

                            }

                        
                        }


                    }

                        
                        







                 }

                TaskDialog.Show("Steel Deck Identification", $"{counter} Steel Deck Profiles have been identified in the floor types");


                t.Commit();
            }
           
            return Result.Succeeded;
        }
    }
}
