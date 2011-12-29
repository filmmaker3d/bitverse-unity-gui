using System;
using Bitverse.Unity.Gui;
using UnityEngine;
using Object = UnityEngine.Object;

public class Transport : UnityMonoBehaviour
{
    private const string GuiPrefabsPath = "gui_prefabs/";
    private const string GuiAssetsPath = "gui_assets/";

    private const string EscPrefab = "esc_window";
    private const string MainMenuPrefab = "mainmenu_window";
    private const string OptionsPrefab = "options_window";
    private const string NewGamePrefab = "newgame_window";
    private const string TopPrefab = "top_window";
    private const string MarketPrefab = "market_window";
    private const string DepotPrefab = "depot_window";

    private const int BalanceDefault = 100000;

    private GameObject _mainMenuWindow;
    private GameObject _optionsMenuWindow;

    private BitStage _stage;
    private MainmenuWindowGuiAcessor _mainWindowAccessor;
    private OptionsWindowGuiAcessor _optionsWindowAccessor;

    #region Startup

    public override void Start()
    {
        Debug.Log("Start =D");
        
        Debug.Log("Loading BitStage...");
        Object obj = FindObjectOfType(typeof(BitStage));
        if (obj == null)
        {
            Debug.LogError("Failed to load BitStage");
            return;
        }

        _stage = (BitStage)obj;

        StartMainWindowComponents(OpenWindow(MainMenuPrefab));

    }

    #endregion

    #region Windows

    #region 0 - Esc window

    private bool _escOpen;

    private void OpenEscWindow()
    {
        if (_escOpen)
        {
            UnloadWindow(EscPrefab);
            _escOpen = false;
            return;
        }

        var accessor = new EscWindowGuiAcessor(OpenWindow(EscPrefab));
        accessor.ExitgameButton.MouseClick += (sender, e) => Application.Quit();
        accessor.BacktogameButton.MouseClick +=
            (sender, e) =>
                {
                    UnloadWindow(EscPrefab);
                    _escOpen = false;
                };

        _escOpen = true;
    }

    #endregion

    #region 1 - Main menu
    private void StartMainWindowComponents(BitWindow window)
    {
        var accessor = new MainmenuWindowGuiAcessor(window);
        accessor.NewgameButton.MouseClick +=
           (sender, e) =>
           {
               UnloadWindow(MainMenuPrefab);
               StartNewGameWindowComponents(OpenWindow(NewGamePrefab));
           };

        accessor.OptionsButton.MouseClick +=
           (sender, e) =>
           {
               UnloadWindow(MainMenuPrefab);
               StartOptionsWindowComponents(OpenWindow(OptionsPrefab));
           };

        accessor.ExitButton.MouseClick += (sender, e) => Application.Quit();
    }
    #endregion

    #region 1 - New game

    private Player _player;
    private string _playerName;

    private void StartNewGameWindowComponents(BitWindow window)
    {
        var accessor = new NewgameWindowGuiAcessor(window);

        accessor.StartButton.Enabled = false;

        accessor.NameTextfield.TextChanged +=
           (sender, e) =>
           {
               if (string.IsNullOrEmpty(accessor.NameTextfield.Text))
               {
                   accessor.StartButton.Enabled = false;
                   return;
               }

               _playerName = accessor.NameTextfield.Text;
               accessor.StartButton.Enabled = true;
           };

        accessor.BackButton.MouseClick +=
           (sender, e) =>
           {
               UnloadWindow(NewGamePrefab);
               StartMainWindowComponents(OpenWindow(MainMenuPrefab));
           };

        accessor.StartButton.MouseClick +=
           (sender, e) =>
           {
               _player = new Player(_playerName, BalanceDefault);
               UnloadWindow(NewGamePrefab);
               StartGameWindows();
           };
    }
    #endregion

    #region 1 - Options
    private void StartOptionsWindowComponents(BitWindow window)
    {
        var accessor = new OptionsWindowGuiAcessor(window);

        accessor.ResOpt1Toggle.SetValue(false);
        accessor.ResOpt2Toggle.SetValue(false);
        accessor.ResOpt3Toggle.SetValue(false);
        var width = Screen.width;
        switch (width)
        {
            case 1280:
                accessor.ResOpt2Toggle.SetValue(true);
                break;
            case 1440:
                accessor.ResOpt3Toggle.SetValue(true);
                break;
            default:
                accessor.ResOpt1Toggle.SetValue(true);
                break;
        }

        accessor.BackButton.MouseClick +=
           (sender, e) =>
           {
               UnloadWindow(OptionsPrefab);
               StartMainWindowComponents(OpenWindow(MainMenuPrefab));
           };

        accessor.ResOpt1Toggle.ValueChanged +=
           (sender, e) =>
           {
               if ((bool)e.Value)
                   ((BitToggle)sender).SetValue(true);
               accessor.ResOpt2Toggle.SetValue(false);
               accessor.ResOpt3Toggle.SetValue(false);
               Screen.SetResolution(1024, 768, false);
           };
        accessor.ResOpt2Toggle.ValueChanged +=
           (sender, e) =>
           {
               if ((bool)e.Value)
                   ((BitToggle)sender).SetValue(true);
               accessor.ResOpt1Toggle.SetValue(false);
               accessor.ResOpt3Toggle.SetValue(false);
               Screen.SetResolution(1280, 720, false);
           };
        accessor.ResOpt3Toggle.ValueChanged +=
           (sender, e) =>
           {
               if ((bool)e.Value)
                   ((BitToggle)sender).SetValue(true);
               accessor.ResOpt1Toggle.SetValue(false);
               accessor.ResOpt2Toggle.SetValue(false);
               Screen.SetResolution(1440, 900, false);
           };

    }
    #endregion

    private void StartGameWindows()
    {
        OpenTopWindow();
    }

    #region 2 - Top window

    private bool _marketOpen;
    private bool _depotOpen;
    private BitLabel _balanceLabel;

    private void OpenTopWindow()
    {
        var accessor = new TopWindowGuiAcessor(OpenWindow(TopPrefab));

        accessor.PlayernameLabel.Text = _player.Name;
        accessor.PlayerbalanceLabel.Text = "$ " +_player.Balance;
        _balanceLabel = accessor.PlayerbalanceLabel;

        accessor.MarketButton.MouseClick += 
            (sender, e) =>
            {
                if (_marketOpen)
                {
                    UnloadWindow(MarketPrefab);
                    _marketOpen = false;
                    return;
                }

                OpenMarketWindow();
                _marketOpen = true;
            };

        accessor.DepotButton.MouseClick +=
            (sender, e) =>
            {
                if (_depotOpen)
                {
                    UnloadWindow(DepotPrefab);
                    _depotOpen = false;
                    return;
                }

                OpenDepotWindow();
                _depotOpen = true;
            };
    }

    #endregion

    #region 2 - Market window

    private MarketItemData _selectedMarketItemData;

    private void OpenMarketWindow()
    {
        var accessor = new MarketWindowGuiAcessor(OpenWindow(MarketPrefab));

        accessor.BuyButton.Enabled = false;

        accessor.CloseButton.MouseClick += 
            (sender, e) =>
            {
                UnloadWindow(MarketPrefab);
                _marketOpen = false;
            };

        //items list
        var model = new DefaultBitListModel();
        accessor.ItemList.Populator = new MarketItemListPopulator(accessor);
        var item1 = new MarketItemData("Scania L-111", "45000", "Scania L-111 description.", (Texture2D)Resources.Load(GuiAssetsPath + "ico_truck1"));
        var item2 = new MarketItemData("Volks 17-220", "100000", "Volks 17-220 description.", (Texture2D)Resources.Load(GuiAssetsPath + "ico_truck2"));
        var item3 = new MarketItemData("Ford 2428", "150000", "Ford 2428 description.", (Texture2D)Resources.Load(GuiAssetsPath + "ico_truck3"));
        var item4 = new MarketItemData("Mercedes 1114", "25000", "Mercedes 1114 description.", (Texture2D)Resources.Load(GuiAssetsPath + "ico_truck4"));
        model.Add(item1);
        model.Add(item2);
        model.Add(item3);
        model.Add(item4);
        accessor.ItemList.Model = model;

        accessor.ItemList.SelectionChanged +=
            (sender, e) =>
            {
                var marketItemData = (MarketItemData)e.Selection[0];
                accessor.DescriptionTextarea.Text = marketItemData.Description;
                accessor.BuyButton.Enabled = _player.Balance >= int.Parse(marketItemData.Price);
                _selectedMarketItemData = marketItemData;
            };

        accessor.BuyButton.MouseClick +=
            (sender, e) =>
            {
                accessor.BuyButton.Enabled = false;
                accessor.ItemList.ClearSelection();
                accessor.DescriptionTextarea.Text = "";
                _player.Balance -= int.Parse(_selectedMarketItemData.Price);
                _balanceLabel.Text = "$ " + _player.Balance;
                _selectedMarketItemData = null;
            };
    }

    #endregion

    #region 2 - Depot window

    private void OpenDepotWindow()
    {
        //TODO - depot window
    }

    #endregion

    #endregion

    #region Update

    private bool _escPressed;

    // Update is called once per frame
    public override void Update()
    {
        if (!_escPressed && Input.GetKeyDown(KeyCode.Escape))
        {
            _escPressed = true;
            OpenEscWindow();
        }
        else if (_escPressed && Input.GetKeyUp(KeyCode.Escape))
        {
            _escPressed = false;
        }
    }

    #endregion

    #region Window core

    private BitWindow OpenWindow(string windowPrefab)
    {
        var window = (GameObject)Resources.Load(GuiPrefabsPath + windowPrefab);
        return LoadWindow(window);
    }

    private BitWindow LoadWindow(GameObject go)
    {
        var instantiatedGo = (GameObject)Instantiate(go);
        instantiatedGo.name = instantiatedGo.name.Remove(instantiatedGo.name.IndexOf('('));

        return _stage.AddControl<BitWindow>(instantiatedGo);
    }

    private void UnloadWindow(string windowName)
    {
        _stage.RemoveControl(windowName);
    }

    private void UnloadWindow(GameObject go)
    {
        _stage.RemoveControl(go.name);
    }

    #endregion
}
