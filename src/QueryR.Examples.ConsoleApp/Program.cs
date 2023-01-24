using QueryR;
using QueryR.Examples.ConsoleApp.MenuSystem;
using QueryR.Examples.Data;
using QueryR.QueryModels;
using System.Collections.Generic;
using System.Collections.Specialized;

public class Program
{
    public static Query KerbalQuery = new();
    public static Query CraftQuery = new();

    public static void Main(string[] args)
    {
        var fields = new List<string>
        {
            nameof(Kerbal.Id),
            nameof(Kerbal.FirstName),
            nameof(Kerbal.LastName),
            nameof(Kerbal.AssignedSpaceCraftId),
            $"{nameof(Kerbal.PlanetaryBodiesVisited)}.{nameof(PlanetaryBody.Id)}",
            $"{nameof(Kerbal.PlanetaryBodiesVisited)}.{nameof(PlanetaryBody.Name)}",
            $"{nameof(Kerbal.SnacksOnHand)}.{nameof(KeyValuePair<string, int>.Key)}",
            $"{nameof(Kerbal.SnacksOnHand)}.{nameof(KeyValuePair<string, int>.Value)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.Id)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.CraftName)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.Id)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.FirstName)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.LastName)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.PlanetaryBodiesVisited)}.{nameof(PlanetaryBody.Id)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.PlanetaryBodiesVisited)}.{nameof(PlanetaryBody.Name)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.SnacksOnHand)}.{nameof(KeyValuePair<string, int>.Key)}",
            $"{nameof(Kerbal.AssignedSpaceCraft)}.{nameof(Craft.AssignedKerbals)}.{nameof(Kerbal.SnacksOnHand)}.{nameof(KeyValuePair<string, int>.Value)}",

        };

        var menu = new Menu
        {
            Title = "Welcome to the Kerbal Querying Program",
            GetItems = () => new IMenuItem[]
            {
                new Menu
                {
                    Description = "Query Kerbals",
                    Title = "Kerbal Menu",
                    GetItems = () => new IMenuItem[]
                    {
                        new MenuItem
                        {
                            Description = "Execute Query",
                            Run = () =>
                            {
                                Console.Clear();
                                Console.WriteLine("Listing Kerbals");
                                var queryResult = Instances.Kerbals.AsQueryable().Query(KerbalQuery);
                                var (Count, Items) = queryResult.GetCountAndList();
                                Console.WriteLine(string.Join(Environment.NewLine, Items));
                                Console.WriteLine($"Total Count : {Count}");
                                Console.WriteLine("Press any key to continue...");
                                Console.ReadKey();
                                Console.Clear();
                                return true;
                            }
                        },
                        new Menu
                        {
                            Description = "Manage Filters",
                            Title = "Kerbal Query Filters",
                            GetItems = () => new IMenuItem[]
                            {
                                new MenuItem
                                {
                                    Description = "List Current Filters",
                                    Run = () =>
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Kerbal Query Filters");
                                        Console.WriteLine(string.Join(Environment.NewLine, KerbalQuery.Filters.Select(f=> $"{f.PropertyName} -{f.Operator.Code} \"{f.Value}\"")));
                                        Console.WriteLine("Press any key to continue...");
                                        Console.ReadKey();
                                        Console.Clear();
                                        return true;
                                    }
                                },
                                new MultiPrompt
                                {
                                    Description = "Add Query Filter",
                                    Prompts =
                                    {
                                        new Menu
                                        {
                                            Title = "Choose Field (Normally you can specify whatever you want as long as it works with the operator, this is a sample of the fields for demo purposes.)",
                                            GetItems = () => new List<MenuItem>(fields
                                                .Select(field=> new MenuItem
                                                {
                                                    Description = field,
                                                    Run = () => false
                                                }))
                                        },
                                        new Menu
                                        {
                                            Title = "Choose Operation",
                                            GetItems = () => new List<MenuItem>(FilterOperators.Items
                                                .Select(fo=> new MenuItem
                                                {
                                                    Description = fo.Name,
                                                    Run = () => false
                                                }))
                                        },
                                        new TextPrompt
                                        {
                                            PromptText = "Enter Filter Value",
                                            Description = string.Empty,
                                            OnSuccess = _ => { }
                                        }
                                    },
                                    OnSuccess = prompts =>
                                    {
                                        var propertyName = fields.ElementAt(int.Parse(prompts[0].Response) - 1);
                                        var filterOperator = FilterOperators.Items.ElementAt(int.Parse(prompts[1].Response) - 1);
                                        KerbalQuery.Filters.Add(new Filter
                                        {
                                            PropertyName = propertyName,
                                            Operator = filterOperator,
                                            Value = prompts[2].Response,
                                        });
                                    }
                                },
                                new Menu
                                {
                                    Description = "Remove Query Filter",
                                    Title = "Select Query Filter to Remove",
                                    GetItems = () => new List<IMenuItem>(KerbalQuery.Filters.Select(f => new MenuItem
                                    {
                                        Description = $"{f.PropertyName} -{f.Operator.Code} \"{f.Value}\"",
                                        Run = () =>
                                        {
                                            Console.Clear();
                                            KerbalQuery.Filters.Remove(f);
                                            return false;
                                        }
                                    }).Append(BackMenuItem))
                                },
                                BackMenuItem
                            }
                        },
                        new Menu
                        {
                            Description = "Manage Paging",
                            Title = string.Empty,
                            GetTitle = () => $"Kerbal Query Paging - Current Settings: { (KerbalQuery.PagingOptions == null ? "Unset" : $"Size { KerbalQuery.PagingOptions.PageSize }, Number { KerbalQuery.PagingOptions.PageNumber }") }",
                            GetItems =  () => new IMenuItem[]
                            {
                                new IntPrompt
                                {
                                    Description = "Set Page Size",
                                    PromptText = "Enter Page Size",
                                    OnSuccess = response =>
                                    {
                                        KerbalQuery.PagingOptions ??= new PagingOptions();
                                        KerbalQuery.PagingOptions.PageSize = response;
                                    }
                                },
                                new IntPrompt
                                {
                                    Description = "Set Page Number",
                                    PromptText = "Enter Page Number",
                                    OnSuccess = response =>
                                    {
                                        KerbalQuery.PagingOptions ??= new PagingOptions();
                                        KerbalQuery.PagingOptions.PageNumber = response;
                                    }
                                },
                                new MenuItem
                                {
                                    Description = "Clear Paging",
                                    Run = () =>
                                    {
                                        KerbalQuery.PagingOptions = null;
                                        Console.Clear();
                                        return true;
                                    }
                                },
                                BackMenuItem
                            }
                        },
                        new Menu
                        {
                            Description = "Manage Sorts",
                            Title = "Kerbal Query Sorts",
                            GetItems = () => new IMenuItem[]
                            {
                                new MenuItem
                                {
                                    Description = "List Current Sorts",
                                    Run = () =>
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Kerbal Query Sorts");
                                        Console.WriteLine(string.Join(Environment.NewLine, KerbalQuery.Sorts.Select(f=> $"{f.PropertyName} {(f.IsAscending ? "Ascending" : "Descending")}")));
                                        Console.WriteLine("Press any key to continue...");
                                        Console.ReadKey();
                                        Console.Clear();
                                        return true;
                                    }
                                },
                                new MultiPrompt
                                {
                                    Description = "Add Sort",
                                    Prompts =
                                    {
                                        new Menu
                                        {
                                            Title = "Choose Field (Normally you can specify whatever you want as long as it works with the operator, this is a sample of the fields for demo purposes.)",
                                            GetItems = () => new List<IMenuItem>(fields
                                                .Select(field => new MenuItem
                                                {
                                                    Description = field,
                                                    Run = () => false
                                                }))
                                        },
                                        new Menu
                                        {
                                            Title = "Choose Order",
                                            GetItems = () => new List<IMenuItem>(new[] { "Ascending", "Descending" }
                                                .Select(a => new MenuItem
                                                {
                                                    Description = a,
                                                    Run = () => false
                                                }))
                                        },
                                    },
                                    OnSuccess = prompts =>
                                    {
                                        var propertyName = typeof(Kerbal).GetProperties().Where(mi => mi.Name != nameof(Kerbal.AssignedSpaceCraft)).ElementAt(int.Parse(prompts[0].Response) - 1).Name;
                                        var isAscending = int.Parse(prompts[1].Response) == 1;
                                        KerbalQuery.Sorts.Add(new Sort
                                        {
                                            PropertyName = propertyName,
                                            IsAscending = isAscending
                                        });
                                    }
                                },
                                new Menu
                                {
                                    Description = "Remove Sort",
                                    Title = "Select Sort to Remove",
                                    GetItems = () => new List<IMenuItem>(KerbalQuery.Sorts.Select(s => new MenuItem
                                    {
                                        Description = $"{s.PropertyName} {(s.IsAscending ? "Ascending" : "Descending")}",
                                        Run = () =>
                                        {
                                            KerbalQuery.Sorts.Remove(s);
                                            return false;
                                        }
                                    }).Append(BackMenuItem))
                                },
                                BackMenuItem
                            }
                        },
                        new Menu
                        {
                            Description = "Manage Sparse Fields",
                            Title = "Kerbal Query Sparse Fields",
                            GetItems = () => new IMenuItem[]
                            {
                                new MenuItem
                                {
                                    Description = "List Current Sparse Fields",
                                    Run = () =>
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Kerbal Query Sparse Fields");
                                        Console.WriteLine(string.Join(Environment.NewLine, KerbalQuery.SparseFields.Select(sf=> $"{sf.EntityName} {string.Join(", ", sf.PropertyNames)}")));
                                        Console.WriteLine("Press any key to continue...");
                                        Console.ReadKey();
                                        Console.Clear();
                                        return true;
                                    }
                                },
                                new Menu
                                {
                                    Description = "Add Kerbal Sparse Field",
                                    Title = "Choose Field",
                                    GetItems = () => new List<IMenuItem>(typeof(Kerbal).GetProperties()
                                        .Where(mi => !KerbalQuery.SparseFields.Any(sf=> sf.EntityName == nameof(Kerbal) && sf.PropertyNames.Contains(mi.Name)))
                                        .Select(mi=> new MenuItem
                                        {
                                            Description = mi.Name,
                                            Run = () =>
                                            {
                                                Console.Clear();
                                                var sparseField = KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(Kerbal));
                                                if(sparseField == null)
                                                {
                                                    sparseField = new SparseField{ EntityName = nameof(Kerbal) };
                                                    KerbalQuery.SparseFields.Add(sparseField);
                                                }
                                                sparseField.PropertyNames.Add(mi.Name);
                                                return true;
                                            }
                                        }).Append(BackMenuItem)),
                                },
                                new Menu
                                {
                                    Description = "Remove Kerbal Sparse Field",
                                    Title = "Choose Field",
                                    GetItems = () => new List<IMenuItem>(KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(Kerbal))?.PropertyNames
                                        .Select(pn => new MenuItem
                                        {
                                            Description = pn,
                                            Run = () =>
                                            {
                                                Console.Clear();
                                                var kerbalSparse = KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(Kerbal));
                                                kerbalSparse?.PropertyNames.Remove(pn);
                                                if(!kerbalSparse.PropertyNames.Any())
                                                {
                                                    KerbalQuery.SparseFields.Remove(kerbalSparse);
                                                }
                                                return true;
                                            }
                                        }).Append(BackMenuItem) ?? Enumerable.Empty<MenuItem>().Append(BackMenuItem)),
                                },
                                new Menu
                                {
                                    Description = "Add Craft Sparse Field",
                                    Title = "Choose Field",
                                    GetItems = () => new List<IMenuItem>(typeof(Craft).GetProperties()
                                        .Where(mi => !KerbalQuery.SparseFields.Any(sf=> sf.EntityName == nameof(Craft) && sf.PropertyNames.Contains(mi.Name)))
                                        .Select(mi=> new MenuItem
                                        {
                                            Description = mi.Name,
                                            Run = () =>
                                            {
                                                Console.Clear();
                                                var sparseField = KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(Craft));
                                                if(sparseField == null)
                                                {
                                                    sparseField = new SparseField{ EntityName = nameof(Craft) };
                                                    KerbalQuery.SparseFields.Add(sparseField);
                                                }
                                                sparseField.PropertyNames.Add(mi.Name);
                                                return true;
                                            }
                                        }).Append(BackMenuItem)),
                                },
                                new Menu
                                {
                                    Description = "Remove Craft Sparse Field",
                                    Title = "Choose Field",
                                    GetItems = () => new List<IMenuItem>(KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(Craft))?.PropertyNames
                                        .Select(pn => new MenuItem
                                        {
                                            Description = pn,
                                            Run = () =>
                                            {
                                                Console.Clear();
                                                var craftSparse = KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(Craft));
                                                craftSparse?.PropertyNames.Remove(pn);
                                                if(!craftSparse.PropertyNames.Any())
                                                {
                                                    KerbalQuery.SparseFields.Remove(craftSparse);
                                                }
                                                return true;
                                            }
                                        }).Append(BackMenuItem) ?? Enumerable.Empty<MenuItem>().Append(BackMenuItem)),
                                },
                                new Menu
                                {
                                    Description = "Add Planetary Body Sparse Field",
                                    Title = "Choose Field",
                                    GetItems = () => new List<IMenuItem>(typeof(PlanetaryBody).GetProperties()
                                        .Where(mi => !KerbalQuery.SparseFields.Any(sf=> sf.EntityName == nameof(PlanetaryBody) && sf.PropertyNames.Contains(mi.Name)))
                                        .Select(mi=> new MenuItem
                                        {
                                            Description = mi.Name,
                                            Run = () =>
                                            {
                                                Console.Clear();
                                                var sparseField = KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(PlanetaryBody));
                                                if(sparseField == null)
                                                {
                                                    sparseField = new SparseField{ EntityName = nameof(PlanetaryBody) };
                                                    KerbalQuery.SparseFields.Add(sparseField);
                                                }
                                                sparseField.PropertyNames.Add(mi.Name);
                                                return true;
                                            }
                                        }).Append(BackMenuItem)),
                                },
                                new Menu
                                {
                                    Description = "Remove Planetary Body Sparse Field",
                                    Title = "Choose Field",
                                    GetItems = () => new List<IMenuItem>(KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(PlanetaryBody))?.PropertyNames
                                        .Select(pn => new MenuItem
                                        {
                                            Description = pn,
                                            Run = () =>
                                            {
                                                Console.Clear();
                                                var sparseField = KerbalQuery.SparseFields.FirstOrDefault(sf=> sf.EntityName == nameof(PlanetaryBody));
                                                sparseField?.PropertyNames.Remove(pn);
                                                if(!sparseField.PropertyNames.Any())
                                                {
                                                    KerbalQuery.SparseFields.Remove(sparseField);
                                                }
                                                return true;
                                            }
                                        }).Append(BackMenuItem) ?? Enumerable.Empty<MenuItem>().Append(BackMenuItem)),
                                },
                                BackMenuItem
                            }
                        },
                        BackMenuItem
                    }
                },
                new MenuItem
                {
                    Description = "Quit",
                    Run = () => false
                }
            }
        };

        menu.Run();
    }


    public static MenuItem BackMenuItem => new MenuItem
    {
        Description = "Back",
        Run = () =>
        {
            Console.Clear();
            return false;
        }
    };
}


