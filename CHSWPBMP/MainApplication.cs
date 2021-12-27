using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace CHSWPBMP
{
    public class MainApplication
    {
        public Config SelectedConfig;
        private int currentIndex = 0;
        public void Run()
        {
            showInitialMenu();
        }

        private void showInitialMenu()
        {
            MenuBuilder builder = new("Config Menu");
            builder.addOption("Create New Configuration");
            builder.addOption("Open Existing Configuration");
            builder.createMenu();

            switch (builder.getResponse().Value)
            {
                case 1:
                    Config newConfig = new(MenuUtility.GetInput("Enter Config Name"));
                    SelectedConfig = newConfig;
                    break;
                case 2:
                    string path = new(MenuUtility.GetInput("Enter Path To Config"));
                    Config loadedConfig;
                    using (FileStream fs = new(path, FileMode.Open))
                    {
                        XmlSerializer xml = new (typeof(Config));
                        loadedConfig = (Config)xml.Deserialize(fs);
                    }

                    SelectedConfig = loadedConfig;
                    break;
            }

            showMainMenu();
        }

        private void showMainMenu()
        {
            while (true)
            {
                MenuBuilder mainMenuBuilder = new("Configuration Builder");
                mainMenuBuilder.addButton("Create Waypoint", ConsoleKey.W);
                mainMenuBuilder.addButton("Upload to Robot 13092", ConsoleKey.U);
                mainMenuBuilder.addButton("Save & Exit", ConsoleKey.S);

                mainMenuBuilder.ClearOptions();
                foreach (Waypoint waypoint in SelectedConfig.Waypoints)
                {
                    mainMenuBuilder.addOption(
                        $"WAYPOINT @ {waypoint.startPose.x}, {waypoint.startPose.y}, {waypoint.startPose.t}  TO  {waypoint.endPose.x}, {waypoint.endPose.y}, {waypoint.endPose.t}");
                }
                mainMenuBuilder.createMenu();
               

                MenuBuilder.Response response = mainMenuBuilder.getResponse();

                if (response.Type == "OPTION")
                {
                    try
                    {
                        editWaypoint(response.Value);
                    } catch {}
                }

                if (response.Type == "BUTTON")
                {
                    switch (response.Key)
                    {
                        case ConsoleKey.W:
                            addWaypoint(currentIndex);
                            currentIndex++;
                            break;
                        case ConsoleKey.S:
                            string path = new(MenuUtility.GetInput("Enter Path To Save"));
                            using (FileStream fs = new(path, FileMode.Create))
                            {
                                XmlSerializer xml = new (typeof(Config));
                                xml.Serialize(fs, SelectedConfig);
                            }
                            break;
                    }
                }
            }
        }

        private void editWaypoint(int index)
        {
            Waypoint waypoint = SelectedConfig.Waypoints[index - 1];

            bool notDone = true;

            while (notDone)
            {
                MenuBuilder editMenuBuilder = new("Edit Waypoint");
                editMenuBuilder.addButton("Delete Waypoint", ConsoleKey.X);
                editMenuBuilder.addButton("Move Waypoint Up", ConsoleKey.UpArrow);
                editMenuBuilder.addButton("Move Waypoint Down", ConsoleKey.DownArrow);
                editMenuBuilder.addButton("Done", ConsoleKey.Enter);
                editMenuBuilder.addOption("Change Start Position");
                editMenuBuilder.addOption("Change End Position");
                editMenuBuilder.addOption("Modify Actions");
                editMenuBuilder.createMenu();

                MenuBuilder.Response editResponse = editMenuBuilder.getResponse();
                if (editResponse.Type == "BUTTON")
                {
                    switch (editResponse.Key)
                    {
                        case ConsoleKey.X:
                            for (int i = 0; i < SelectedConfig.Waypoints.Count; i++)
                            {
                                if (SelectedConfig.Waypoints[i].Index > waypoint.Index)
                                {
                                    SelectedConfig.Waypoints[i].Index -= 1;
                                }
                            }
                            SelectedConfig.Waypoints.RemoveAt(index - 1);
                            notDone = false;
                            break;
                        case ConsoleKey.UpArrow:
                            SelectedConfig.Waypoints[index - 1].Index -= 1;
                            try
                            {
                                SelectedConfig.Waypoints[index - 2].Index += 1;
                            } catch {}

                            try
                            {
                                SelectedConfig.Waypoints.Move(index -1, index -2);
                            } catch {}

                            return;
                            break;
                        case ConsoleKey.DownArrow:
                            SelectedConfig.Waypoints[index - 1].Index += 1;

                            try
                            {
                                SelectedConfig.Waypoints[index].Index -= 1;
                            } catch {}

                            try
                            {
                                SelectedConfig.Waypoints.Move(index -1, index);
                            } catch {}

                            return;
                            break;
                        case ConsoleKey.Enter:
                            notDone = false;
                            break;
                    }
                }

                if (editResponse.Type == "OPTION")
                {
                    switch (editResponse.Value)
                    {
                        case 1:
                            bool startPosDone = false;
                            while (!startPosDone)
                            {
                                MenuBuilder startPositionMenu = new("Change Start Position");
                                startPositionMenu.addButton("Done", ConsoleKey.Enter);
                                startPositionMenu.addOption($"X - {waypoint.startPose.x}");
                                startPositionMenu.addOption($"Y - {waypoint.startPose.y}");
                                startPositionMenu.addOption($"Theta - {waypoint.startPose.t}");
                                startPositionMenu.createMenu();
                                MenuBuilder.Response startPosResponse = startPositionMenu.getResponse();

                                if (startPosResponse.Type == "BUTTON")
                                    if (startPosResponse.Key == ConsoleKey.Enter)
                                        startPosDone = true;

                                if (startPosResponse.Type == "OPTION")
                                {
                                    switch (startPosResponse.Value)
                                    {
                                        case 1:
                                            double x = Convert.ToDouble(MenuUtility.GetInput("Enter New X"));
                                            waypoint.startPose.x = x;
                                            break;
                                        case 2:
                                            double y = Convert.ToDouble(MenuUtility.GetInput("Enter New Y"));
                                            waypoint.startPose.y = y;
                                            break;
                                        case 3:
                                            double t = Convert.ToDouble(MenuUtility.GetInput("Enter New Theta"));
                                            waypoint.startPose.t = t;
                                            break;
                                    }
                                }
                            }
                            break;
                        case 2:
                            bool endPosDone = false;
                            while (!endPosDone)
                            {
                                MenuBuilder endPositionMenu = new("Change Start Position");
                                endPositionMenu.addButton("Done", ConsoleKey.Enter);
                                endPositionMenu.addOption($"X - {waypoint.startPose.x}");
                                endPositionMenu.addOption($"Y - {waypoint.startPose.y}");
                                endPositionMenu.addOption($"Theta - {waypoint.startPose.t}");
                                endPositionMenu.createMenu();
                                MenuBuilder.Response endPosResponse = endPositionMenu.getResponse();

                                if (endPosResponse.Type == "BUTTON")
                                    if (endPosResponse.Key == ConsoleKey.Enter)
                                        endPosDone = true;

                                if (endPosResponse.Type == "OPTION")
                                {
                                    switch (endPosResponse.Value)
                                    {
                                        case 1:
                                            double x = Convert.ToDouble(MenuUtility.GetInput("Enter New X"));
                                            waypoint.endPose.x = x;
                                            break;
                                        case 2:
                                            double y = Convert.ToDouble(MenuUtility.GetInput("Enter New Y"));
                                            waypoint.endPose.y = y;
                                            break;
                                        case 3:
                                            double t = Convert.ToDouble(MenuUtility.GetInput("Enter New Theta"));
                                            waypoint.endPose.t = t;
                                            break;
                                    }
                                }
                            }
                            break;
                        case 3:
                            bool modifyActionsDone = false;
                            while (!modifyActionsDone)
                            {
                                MenuBuilder actionBuilder = new ("Modify Actions");
                                actionBuilder.addButton("Create", ConsoleKey.C);
                                actionBuilder.addButton("Remove", ConsoleKey.R);
                                actionBuilder.addButton("Done", ConsoleKey.Enter);

                                foreach (string action in waypoint.Actions)
                                    actionBuilder.addOption(action);

                                actionBuilder.createMenu();

                                MenuBuilder.Response response = actionBuilder.getResponse();

                                if (response.Type == "BUTTON")
                                {
                                    switch (response.Key)
                                    {
                                        case ConsoleKey.C:
                                            MenuBuilder actionAdd = new("Select Action to Add");
                                            actionAdd.addOption("SPIN_CAROUSEL");
                                            actionAdd.addOption("PLACE_PRELOAD");
                                            actionAdd.addOption("PLACE_CUBE_FROM_WAREHOUSE");
                                            actionAdd.createMenu();
                                            MenuBuilder.Response actionResponse = actionAdd.getResponse();

                                            switch (actionResponse.Value)
                                            {
                                                case 1:
                                                    waypoint.Actions.Add("SPIN_CAROUSEL");
                                                    break;
                                                case 2:
                                                    waypoint.Actions.Add("PLACE_PRELOAD");
                                                    break;
                                                case 3:
                                                    waypoint.Actions.Add("PLACE_CUBE_FROM_WAREHOUSE");
                                                    break;
                                            }

                                            break;
                                        case ConsoleKey.R:
                                            MenuBuilder actionRemoval = new("Select Action to Remove");
                                            foreach (string action in waypoint.Actions)
                                            {
                                                actionRemoval.addOption(action);
                                            }

                                            try
                                            {
                                                actionRemoval.createMenu();
                                                MenuBuilder.Response removalResponse = actionRemoval.getResponse();
                                                waypoint.Actions.RemoveAt(removalResponse.Value - 1);
                                            } catch {}
                                            break;
                                        case ConsoleKey.Enter:
                                            modifyActionsDone = true;
                                            break;
                                    }
                                }
                            }
                            break;
                    }
                }
            }

            SelectedConfig.Waypoints[index - 1] = waypoint;
        }

        private void addWaypoint(int index)
        {
            double sX = Convert.ToDouble(MenuUtility.GetInput("Enter Start Position X"));
            double sY = Convert.ToDouble(MenuUtility.GetInput("Enter Start Position Y"));
            double sT = Convert.ToDouble(MenuUtility.GetInput("Enter Start Position Theta"));
            double eX = Convert.ToDouble(MenuUtility.GetInput("Enter End Position X"));
            double eY = Convert.ToDouble(MenuUtility.GetInput("Enter End Position Y"));
            double eT = Convert.ToDouble(MenuUtility.GetInput("Enter End Position Theta"));

            Position startPos = new()
            {
                x = sX,
                y = sY,
                t = sT
            };

            Position endPos = new()
            {
                x = eX,
                y = eY,
                t = eT
            };
            
            bool notDone = true;

            Waypoint waypoint = new();

            waypoint.Index = index;
            waypoint.Actions = new List<string>();
            waypoint.startPose = startPos;
            waypoint.endPose = endPos;

            while (notDone)
            {
                MenuBuilder actionBuilder = new ("Add Actions");
                actionBuilder.addButton("Create", ConsoleKey.C);
                actionBuilder.addButton("Remove", ConsoleKey.R);
                actionBuilder.addButton("Done", ConsoleKey.Enter);

                foreach (string action in waypoint.Actions)
                    actionBuilder.addOption(action);

                actionBuilder.createMenu();
                MenuBuilder.Response response = actionBuilder.getResponse();
                if (response.Type == "BUTTON")
                {
                    switch (response.Key)
                    {
                        case ConsoleKey.C:
                            MenuBuilder actionAdd = new("Select Action to Add");
                            actionAdd.addOption("SPIN_CAROUSEL");
                            actionAdd.addOption("PLACE_PRELOAD");
                            actionAdd.addOption("PLACE_CUBE_FROM_WAREHOUSE");
                            actionAdd.createMenu();
                            MenuBuilder.Response actionResponse = actionAdd.getResponse();
                            switch (actionResponse.Value)
                            {
                                case 1:
                                    waypoint.Actions.Add("SPIN_CAROUSEL");
                                    break;
                                case 2:
                                    waypoint.Actions.Add("PLACE_PRELOAD");
                                    break;
                                case 3:
                                    waypoint.Actions.Add("PLACE_CUBE_FROM_WAREHOUSE");
                                    break;
                            }
                            break;
                        case ConsoleKey.R:
                            MenuBuilder actionRemoval = new("Select Action to Remove");
                            foreach (string action in waypoint.Actions)
                            {
                                actionRemoval.addOption(action);
                            }

                            try
                            {
                                actionRemoval.createMenu();
                                MenuBuilder.Response removalResponse = actionRemoval.getResponse();
                                waypoint.Actions.RemoveAt(removalResponse.Value - 1);
                            } catch {}
                            break;
                        case ConsoleKey.Enter:
                            notDone = false;
                            break;
                    }
                }
            }

            SelectedConfig.Waypoints.Add(waypoint);

        }
    }
}
