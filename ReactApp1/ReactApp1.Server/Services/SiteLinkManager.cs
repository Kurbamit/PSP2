using ReactApp1.Server.Models.Enums;
using ReactApp1.Server.Models.Models.Base;
using System.Collections.Generic;

namespace ReactApp1.Server.Services
{
    public static class SiteLinkManager
    {
        public static NavigationModel GetNavigationBasedOnRole(TitleEnum? userRole)
        {
            var navigation = new NavigationModel();

            if (userRole == TitleEnum.MasterAdmin)
            {
                navigation.Items.AddRange(new List<NavigationItem>
                {
                    new NavigationItem { Path = "/orders", Label = "Orders" },
                    new NavigationItem { Path = "/reservations", Label = "Reservations" },
                    new NavigationItem { Path = "/items", Label = "Items" },
                    new NavigationItem { Path = "/inventory", Label = "Inventory" },
                    new NavigationItem { Path = "/services", Label = "Services" },
                    new NavigationItem { Path = "/employees", Label = "Employees" },
                    new NavigationItem { Path = "/taxes", Label = "Taxes" },
                    new NavigationItem { Path = "/giftcards", Label = "Giftcards" },

                    new NavigationItem
                    {
                        Label = "Settings",
                        IsDropdown = true,
                        DropdownItems = new List<NavigationItem>
                        {
                            new NavigationItem { Path = "/settings/profile", Label = "Profile" },
                            new NavigationItem { Path = "/settings/preferences", Label = "Preferences" }
                        }
                    },
                    new NavigationItem { Path = "/logout", Label = "Logout" }
                });
            }
            else if (userRole == TitleEnum.Manager)
            {
                navigation.Items.AddRange(new List<NavigationItem>
                {
                    new NavigationItem { Path = "/items", Label = "Items" },
                    new NavigationItem
                    {
                        Label = "Settings",
                        IsDropdown = true,
                        DropdownItems = new List<NavigationItem>
                        {
                            new NavigationItem { Path = "/settings/profile", Label = "Profile" }
                        }
                    },
                    new NavigationItem { Path = "/logout", Label = "Logout" }
                });
            }
            else
            {
                // Default items for unauthenticated users
                navigation.Items.AddRange(new List<NavigationItem>
                {
                    new NavigationItem { Path = "/register", Label = "Register" },
                    new NavigationItem { Path = "/login", Label = "Login" }
                });
            }

            return navigation;
        }
    }
}
