using Bitverse.Unity.Gui;
using UnityEngine;


public class TestMain : MonoBehaviour
{
    private BitEditorStage _form;
	private BitWindow _window;
	private BitButton _simpleButton;
	private BitRepeatButton _repeatButton;
	private BitToggle _toggle;
	private BitGroup _textGroup;
	private BitTextField _textField;
	private BitTextArea _textArea;
	private BitPasswordField _passwordField;
	private BitHorizontalSlider _horizontalSlider;
	private BitLabel _hsLabel;
	private BitLabel _vsLabel;
	private BitVerticalSlider _verticalSlider;
	private BitGroup _textureGroup;
	//private BitDrawTexture _texture;
	private BitPopup _dropDownOptions;
	private BitGridList _gridList;
	private BitList _list;
	private BitLabel _labelWithPopup;

	private void Start()
	{
		_form = gameObject.GetComponent<BitEditorStage>();
		if (_form == null)
		{
			Debug.LogError("Form not found");
			return;
		}


		_window = _form.FindControl<BitWindow>("main_window");
		if (_window == null)
		{
			Debug.LogError("'main_window' not found");
			return;
		}

		_simpleButton = _window.FindControl<BitButton>("SimpleButton");

		if (_simpleButton == null)
		{
			Debug.LogWarning("'SimpleButton' not found!");
		}
		else
		{
            _simpleButton.MouseClick += SimpleButton_MouseClick;
		}

		_repeatButton = _window.FindControl<BitRepeatButton>("RepeatButton");
		if (_repeatButton == null)
		{
			Debug.LogWarning("'RepeatButton' not found!");
		}
		else
		{
			_repeatButton.MouseHold += RepeatButtonMouseHold;
		}

		_textGroup = _window.FindControl<BitGroup>("TextGroup");
		if (_textGroup == null)
		{
			Debug.Log("'TextGroup' not fount!");
		}

		_toggle = _window.FindControl<BitToggle>("Toggle");
		if (_toggle == null)
		{
			Debug.LogWarning("'Toggle' not found!");
		}
		else
		{
			_toggle.ValueChanged += Toggle_ValueChanged;
			if (_textGroup != null)
			{
				_textGroup.Enabled = false;
			}
		}

		if (_textGroup != null)
		{
			_textField = _textGroup.FindControl<BitTextField>("TextField");
			if (_textField == null)
			{
				Debug.LogWarning("'TextField' not found!");
			}
			else
			{
				_textField.TextChanged += TextChanged;
			}

			_textArea = _textGroup.FindControl<BitTextArea>("TextArea");
			if (_textArea == null)
			{
				Debug.LogWarning("'TextArea' not found!");
			}
			else
			{
				_textArea.TextChanged += TextChanged;
			}

			_passwordField = _textGroup.FindControl<BitPasswordField>("PasswordField");
			if (_passwordField == null)
			{
				Debug.LogWarning("'PasswordField' not found!");
			}
			else
			{
				_passwordField.TextChanged += TextChanged;
			}
		}

		_textureGroup = _window.FindControl<BitGroup>("TextureGroup");
		if (_textureGroup == null)
		{
			Debug.LogWarning("'TextureGroup' not found!");
		}
		else
		{
//			_texture = _textureGroup.FindControl<BitDrawTexture>("Texture");
//			if (_texture == null)
//			{
//				Debug.LogWarning("'Texture' not found!");
//			}
		}

		_hsLabel = _window.FindControl<BitLabel>("HorizontalSlider Label");
		if (_hsLabel == null)
		{
			Debug.LogWarning("'HorizontalSlider Label' not found!");
		}

		_horizontalSlider = _window.FindControl<BitHorizontalSlider>("HorizontalSlider");
		if (_horizontalSlider == null)
		{
			Debug.LogWarning("'HorizontalSlider' not found!");
		}
		else
		{
			_horizontalSlider.ValueChanged += HorizontalSlider_ValueChanged;
		}

		_vsLabel = _window.FindControl<BitLabel>("VerticalSlider Label");
		if (_vsLabel == null)
		{
			Debug.LogWarning("'VerticalSlider Label' not found!");
		}

		_verticalSlider = _window.FindControl<BitVerticalSlider>("VerticalSlider");
		if (_verticalSlider == null)
		{
			Debug.LogWarning("'HorizontalSlider' not found!");
		}
		else
		{
			_verticalSlider.ValueChanged += VerticalSlider_ValueChanged;
		}

		_gridList = _window.FindControl<BitGridList>("GridList");
		if (_gridList == null)
		{
			Debug.LogWarning("'GridList' not found!");
		}
		else
		{
			_gridList.Populator = new DefaultBitListPopulator();
		}

		_multiselectionToggle = _window.FindControl<BitToggle>("MultiSelection Toggle");
		if (_multiselectionToggle == null)
		{
			Debug.LogWarning("'MultiSelection Toggle' not found!");
		}
		else
		{
			_multiselectionToggle.ValueChanged += MultiselectionToggle_ValueChanged;
		}

		_list = _window.FindControl<BitList>("List");
		if (_list == null)
		{
			Debug.LogWarning("'List' not found!");
		}
		else
		{
			_list.Populator = new MyListPopulator();
			_list.SelectionChanged += List_SelectionChanged;
		}

		_listSelectionLabel = _window.FindControl<BitLabel>("ListSelection Label");
		if (_listSelectionLabel == null)
		{
			Debug.LogWarning("'ListSelection Label' not found!");
		}

		_labelWithPopup = _window.FindControl<BitLabel>("Label With Popup");
		if (_labelWithPopup == null)
		{
			Debug.LogWarning("'Label With Popup' not found!");
		}

		PopulateDropDown();

		PopulateContextMenu();
	}

    private void PopulateContextMenu()
	{
		_labelContextMenu = _form.FindControl<BitPopup>("Label Context Menu");
		if (_labelContextMenu == null)
		{
			Debug.LogWarning("'Label Context Menu' Popup not found!");
		}
		else
		{
			BitList options = _labelContextMenu.Options;
			if (options == null)
			{
				Debug.LogWarning("'Label Context Menu > Options' not found");
				return;
			}

			DefaultBitListModel model = new DefaultBitListModel();
			model.Add("Happy :)");
			model.Add("Laugh :D");
			model.Add("Sad :(");
			model.Add("Angry >:(");
			model.Add("Kawaii ^_^");
			model.Add("Stay calm -_-");
			model.Add("Dead +_+");
			model.Add("... ¬¬\"");

			options.Model = model;
			options.Populator = new DefaultBitListPopulator();

			_labelContextMenu.SelectionChanged += LabelContextMenu_SelectionChanged;
		}
	}


	private void PopulateDropDown()
	{
		_dropDownOptions = _form.FindControl<BitPopup>("DropDown Options");
		if (_dropDownOptions == null)
		{
			Debug.LogWarning("'DropDown Options' Popup not found!");
		}
		else
		{
			BitList options = _dropDownOptions.Options;
			if (options == null)
			{
				Debug.LogWarning("'DropDown Options > Options' not found");
				return;
			}

			DefaultBitListModel model = new DefaultBitListModel();

			CreateListModels();
			model.Add(new NameModel("Amphibians", _amphibiansListModel));
			model.Add(new NameModel("Birds", _birdsListModel));
			model.Add(new NameModel("Bugs", _bugsListModel));
			model.Add(new NameModel("Fishes", _fishesListModel));
			model.Add(new NameModel("Invertebrates", _invertebratesListModel));
			model.Add(new NameModel("Mammals", _mammalsListModel));
			model.Add(new NameModel("Prehistorics", _prehistoricsListModel));
			model.Add(new NameModel("Repitiles", _repitilesListModel));

			options.Model = model;
            options.Populator = new PopupPopulator();

			_dropDownOptions.SelectionChanged += DropDownOptions_SelectionChanged;
		}
	}

    private class PopupPopulator : IPopulator
    {
        public void Populate(BitControl renderer, object data, int index, bool selected)
        {
            string name = ((NameModel)data).Name;
            Debug.Log(name);
            renderer.Text = name;
        }
    }

	#region Events

	private void SimpleButton_MouseClick(object sender, MouseEventArgs e)
	{
		Debug.Log("Simple button clicked");
	}

	private void RepeatButtonMouseHold(object sender, MouseEventArgs e)
	{
		Debug.Log("Holding 'RepeatButton'");
	}

	private void Toggle_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		if (_textGroup != null)
		{
			_textGroup.Enabled = (bool)e.Value;
		}
		else
		{
			Debug.Log("Value changed for 'Toggle': " + e.Value);
		}
	}

	private void TextChanged(object sender, ValueChangedEventArgs e)
	{
		Debug.Log("'" + ((BitControl)sender).name + "' text changed to: " + e.Value);
	}

	private void HorizontalSlider_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		if (_hsLabel != null)
		{
			_hsLabel.Content.text = string.Format("{0:F}", e.Value);
		}
		else
		{
			Debug.Log("Value changed for 'HorizontalSlider': " + e.Value);
		}

//		if (_texture != null)
//		{
//			_texture.Location = new Point((float)e.Value, _texture.Position.y);
//		}
	}

	private void VerticalSlider_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		if (_vsLabel != null)
		{
			_vsLabel.Content.text = string.Format("{0:F}", e.Value);
		}
		else
		{
			Debug.Log("Value changed for 'VerticalSlider': " + e.Value);
		}

//		if (_texture != null)
//		{
//			_texture.Location = new Point(_texture.Position.x, (float)e.Value);
//		}
	}

	private void DropDownOptions_SelectionChanged(object sender, SelectionChangedEventArgs<object> e)
	{
		if (_list != null)
		{
			_list.Model = ((NameModel)e.Selection[0]).Model;
		}
		else
		{
			Debug.Log("DropDown selection changed!");
		}

		if (_gridList != null)
		{
			_gridList.Model = ((NameModel)e.Selection[0]).Model;
		}
	}

	private void List_SelectionChanged(object sender, SelectionChangedEventArgs<object> e)
	{
		if (_listSelectionLabel != null)
		{
			_listSelectionLabel.Content.text = e.Selection[0].ToString();
		}
		else
		{
			Debug.Log("List selection changed.");
		}
	}

	void MultiselectionToggle_ValueChanged(object sender, ValueChangedEventArgs e)
	{
		if (_gridList == null)
		{
			return;
		}
		_gridList.MultiSelection = (bool)e.Value;
	}

	void LabelContextMenu_SelectionChanged(object sender, SelectionChangedEventArgs<object> e)
	{
		if (_labelWithPopup != null)
		{
			string s = e.Selection[0].ToString();
			_labelWithPopup.Content.text = "Right click here " + s.Substring(s.LastIndexOf(' '));
		}
	}

	#endregion


	#region ListModels

	private class NameModel
	{
		public readonly string Name;
		public readonly DefaultBitListModel Model;

		public NameModel(string name, DefaultBitListModel model)
		{
			Name = name;
			Model = model;
		}

		public override string ToString()
		{
			return Name;
		}
	}


	public class MyListPopulator : IPopulator
	{
		public void Populate(BitControl listRenderer, object data, int index, bool selected)
		{
			listRenderer.Content.text = data.ToString();
			((BitToggle)listRenderer).Value = selected;
		}
	}



	private DefaultBitListModel _amphibiansListModel;
	private DefaultBitListModel _birdsListModel;
	private DefaultBitListModel _bugsListModel;
	private DefaultBitListModel _fishesListModel;
	private DefaultBitListModel _invertebratesListModel;
	private DefaultBitListModel _mammalsListModel;
	private DefaultBitListModel _prehistoricsListModel;
	private DefaultBitListModel _repitilesListModel;
	private BitLabel _listSelectionLabel;
	private BitToggle _multiselectionToggle;
	private BitPopup _labelContextMenu;

	private void CreateListModels()
	{
		_amphibiansListModel = new DefaultBitListModel();
		_amphibiansListModel.Add("Amazon Horned Frog");
		_amphibiansListModel.Add("Cane Toad");
		_amphibiansListModel.Add("Golden Poison Dart Frog");
		_amphibiansListModel.Add("Green-Eyed Tree Frog");
		_amphibiansListModel.Add("Mexican Axolotl");
		_amphibiansListModel.Add("Mudpuppy");
		_amphibiansListModel.Add("Northern Leopard Frog");
		_amphibiansListModel.Add("Oriental Fire-Bellied Toad");
		_amphibiansListModel.Add("Poison Dart Frog");
		_amphibiansListModel.Add("Red-Eyed Tree Frog");
		_amphibiansListModel.Add("Spotted Salamander");
		_amphibiansListModel.Add("Spring Peeper");
		_amphibiansListModel.Add("Tiger Salamander");
		_amphibiansListModel.Add("Wallace's Flying Frog");
		_amphibiansListModel.Add("Warty Newt");

		_birdsListModel = new DefaultBitListModel();
		_birdsListModel.Add("Adélie Penguin");
		_birdsListModel.Add("Albatross");
		_birdsListModel.Add("Arctic Skua");
		_birdsListModel.Add("Atlantic Puffin");
		_birdsListModel.Add("Bald Eagle");
		_birdsListModel.Add("Baltimore Oriole");
		_birdsListModel.Add("Bird of Paradise");
		_birdsListModel.Add("Blue Jay");
		_birdsListModel.Add("Blue-Footed Booby");
		_birdsListModel.Add("Bluebird");
		_birdsListModel.Add("California Condor");
		_birdsListModel.Add("Canada Goose");
		_birdsListModel.Add("Carolina Wren");
		_birdsListModel.Add("Common Loon");
		_birdsListModel.Add("Common Sandpiper");
		_birdsListModel.Add("Cuban Screech Owl");
		_birdsListModel.Add("Golden Eagle");
		_birdsListModel.Add("Great Blue Heron");
		_birdsListModel.Add("Great Egret");
		_birdsListModel.Add("Great Horned Owl");
		_birdsListModel.Add("Ivory-Billed Woodpecker");
		_birdsListModel.Add("Laughing Kookaburra");
		_birdsListModel.Add("Macaw");
		_birdsListModel.Add("Mallard Duck");
		_birdsListModel.Add("Ostrich");
		_birdsListModel.Add("Parrot");
		_birdsListModel.Add("Peacock");
		_birdsListModel.Add("Pelican");
		_birdsListModel.Add("Peregrine Falcon");
		_birdsListModel.Add("Pileated Woodpecker");
		_birdsListModel.Add("Quetzal");
		_birdsListModel.Add("Raven");
		_birdsListModel.Add("Red-Footed Booby");
		_birdsListModel.Add("Red-Tailed Hawk");
		_birdsListModel.Add("Ring-Necked Pheasant");
		_birdsListModel.Add("Snow Goose");
		_birdsListModel.Add("Steller's Sea Eagle");
		_birdsListModel.Add("Thick-Billed Murre");
		_birdsListModel.Add("Toucan");
		_birdsListModel.Add("Tundra Swan");
		_birdsListModel.Add("Wood Stork");

		_bugsListModel = new DefaultBitListModel();
		_bugsListModel.Add("Black Widow Spider");
		_bugsListModel.Add("Camel Spider");
		_bugsListModel.Add("Cicada");
		_bugsListModel.Add("Deer Tick");
		_bugsListModel.Add("Firefly");
		_bugsListModel.Add("Honeybee");
		_bugsListModel.Add("Hornet");
		_bugsListModel.Add("Ladybug");
		_bugsListModel.Add("Locust");
		_bugsListModel.Add("Monarch Butterfly");
		_bugsListModel.Add("Mosquito");
		_bugsListModel.Add("Praying Mantis");
		_bugsListModel.Add("Scarab");
		_bugsListModel.Add("Scorpion");
		_bugsListModel.Add("Stick Insect");
		_bugsListModel.Add("Tarantula");

		_fishesListModel = new DefaultBitListModel();
		_fishesListModel.Add("Anglerfish");
		_fishesListModel.Add("Arapaima");
		_fishesListModel.Add("Atlantic Bluefin Tuna");
		_fishesListModel.Add("Bull Shark");
		_fishesListModel.Add("Butterflyfish");
		_fishesListModel.Add("Chinese Paddlefish");
		_fishesListModel.Add("Chinese Sturgeon");
		_fishesListModel.Add("Clown Anemonefish");
		_fishesListModel.Add("Electric Eel");
		_fishesListModel.Add("Flying Fish");
		_fishesListModel.Add("Gar");
		_fishesListModel.Add("Giant Barb");
		_fishesListModel.Add("Giant Freshwater Stingray");
		_fishesListModel.Add("Great White Shark");
		_fishesListModel.Add("Hammerhead Shark");
		_fishesListModel.Add("Lake Sturgeon");
		_fishesListModel.Add("Lionfish");
		_fishesListModel.Add("Mekong Giant Catfish");
		_fishesListModel.Add("Mola (Sunfish)");
		_fishesListModel.Add("Parrot Fish");
		_fishesListModel.Add("Pufferfish");
		_fishesListModel.Add("Queen Angelfish");
		_fishesListModel.Add("Rainbow Trout");
		_fishesListModel.Add("River Catfish");
		_fishesListModel.Add("Sailfish");
		_fishesListModel.Add("Sand Tiger Shark");
		_fishesListModel.Add("Seahorse");
		_fishesListModel.Add("Sockeye Salmon");
		_fishesListModel.Add("Stingray");
		_fishesListModel.Add("Taimen");
		_fishesListModel.Add("Whale Shark");

		_invertebratesListModel = new DefaultBitListModel();
		_invertebratesListModel.Add("Blue Crab");
		_invertebratesListModel.Add("Box Jellyfish");
		_invertebratesListModel.Add("Common Earthworm");
		_invertebratesListModel.Add("Common Octopus");
		_invertebratesListModel.Add("Coral");
		_invertebratesListModel.Add("Geographic Cone Snail");
		_invertebratesListModel.Add("Giant Clam");
		_invertebratesListModel.Add("Giant Pacific Octopus");
		_invertebratesListModel.Add("Giant Squid");
		_invertebratesListModel.Add("Golden Cowrie");
		_invertebratesListModel.Add("Krill");
		_invertebratesListModel.Add("Lobster");
		_invertebratesListModel.Add("Nudibranch");
		_invertebratesListModel.Add("Oyster");
		_invertebratesListModel.Add("Portuguese Man-of-War");
		_invertebratesListModel.Add("Sea Anemone");
		_invertebratesListModel.Add("Sea Cucumber");
		_invertebratesListModel.Add("Starfish (Sea Star)");

		_mammalsListModel = new DefaultBitListModel();
		_mammalsListModel.Add("African Elephant");
		_mammalsListModel.Add("African Lion");
		_mammalsListModel.Add("American Bison");
		_mammalsListModel.Add("Arctic Fox");
		_mammalsListModel.Add("Arctic Hare");
		_mammalsListModel.Add("Asian Lion");
		_mammalsListModel.Add("Aye-Aye");
		_mammalsListModel.Add("Baboon");
		_mammalsListModel.Add("Bactrian Camel");
		_mammalsListModel.Add("Beaver");
		_mammalsListModel.Add("Beluga Whale");
		_mammalsListModel.Add("Bengal Tiger");
		_mammalsListModel.Add("Black-Footed Ferret");
		_mammalsListModel.Add("Blue Whale");
		_mammalsListModel.Add("Bottlenose Dolphin");
		_mammalsListModel.Add("Brown Bear");
		_mammalsListModel.Add("California Sea Lion");
		_mammalsListModel.Add("Caribou");
		_mammalsListModel.Add("Cheetah");
		_mammalsListModel.Add("Chimpanzee");
		_mammalsListModel.Add("Chipmunk");
		_mammalsListModel.Add("Common Wombat");
		_mammalsListModel.Add("Cottontail Rabbit");
		_mammalsListModel.Add("Coyote");
		_mammalsListModel.Add("Dingo");
		_mammalsListModel.Add("Domestic Cat");
		_mammalsListModel.Add("Dromedary");
		_mammalsListModel.Add("Dugong");
		_mammalsListModel.Add("Eastern Gray Kangaroo");
		_mammalsListModel.Add("Elk");
		_mammalsListModel.Add("Fennec Fox");
		_mammalsListModel.Add("Fossa");
		_mammalsListModel.Add("Fur Seal");
		_mammalsListModel.Add("Gelada");
		_mammalsListModel.Add("Giant Anteater");
		_mammalsListModel.Add("Giant Panda");
		_mammalsListModel.Add("Giant River Otter");
		_mammalsListModel.Add("Gray Whale");
		_mammalsListModel.Add("Grizzly Bear");
		_mammalsListModel.Add("Groundhog");
		_mammalsListModel.Add("Harbor Porpoise");
		_mammalsListModel.Add("Harp Seal");
		_mammalsListModel.Add("Hawaiian Monk Seal");
		_mammalsListModel.Add("Hedgehog");
		_mammalsListModel.Add("Hippopotamus");
		_mammalsListModel.Add("Howler Monkey");
		_mammalsListModel.Add("Humpback Whale");
		_mammalsListModel.Add("Impala");
		_mammalsListModel.Add("Indian Rhinoceros");
		_mammalsListModel.Add("Jackrabbit");
		_mammalsListModel.Add("Jaguar");
		_mammalsListModel.Add("Kinkajou");
		_mammalsListModel.Add("Koala");
		_mammalsListModel.Add("Leopard");
		_mammalsListModel.Add("Leopard Seal");
		_mammalsListModel.Add("Llama");
		_mammalsListModel.Add("Mandrill");
		_mammalsListModel.Add("Matschie's Tree Kangaroo");
		_mammalsListModel.Add("Meerkat");
		_mammalsListModel.Add("Mole Rat");
		_mammalsListModel.Add("Mongoose");
		_mammalsListModel.Add("Moose");
		_mammalsListModel.Add("Mountain Goat");
		_mammalsListModel.Add("Mountain Gorilla");
		_mammalsListModel.Add("Mountain Lion");
		_mammalsListModel.Add("Mouse Lemur");
		_mammalsListModel.Add("Musk-Ox");
		_mammalsListModel.Add("Narwhal");
		_mammalsListModel.Add("Nutria");
		_mammalsListModel.Add("Ocelot");
		_mammalsListModel.Add("Opossum");
		_mammalsListModel.Add("Orangutan");
		_mammalsListModel.Add("Ozark Big-Eared Bat");
		_mammalsListModel.Add("Platypus");
		_mammalsListModel.Add("Polar Bear");
		_mammalsListModel.Add("Porcupine");
		_mammalsListModel.Add("Prairie Dog");
		_mammalsListModel.Add("Pronghorns");
		_mammalsListModel.Add("Raccoon");
		_mammalsListModel.Add("Red Kangaroo");
		_mammalsListModel.Add("Red Panda");
		_mammalsListModel.Add("Red Uakari");
		_mammalsListModel.Add("Rhesus Monkey");
		_mammalsListModel.Add("Right Whale");
		_mammalsListModel.Add("Ring-Tailed Lemur");
		_mammalsListModel.Add("Ringed Seal");
		_mammalsListModel.Add("Sea Otter");
		_mammalsListModel.Add("Siberian Tiger");
		_mammalsListModel.Add("Sifaka");
		_mammalsListModel.Add("Skunk");
		_mammalsListModel.Add("Sloth Bear");
		_mammalsListModel.Add("Snow Leopard");
		_mammalsListModel.Add("Snowshoe Hare");
		_mammalsListModel.Add("Spectacled Bear");
		_mammalsListModel.Add("Sperm Whale");
		_mammalsListModel.Add("Spotted Hyena");
		_mammalsListModel.Add("Squirrel");
		_mammalsListModel.Add("Steller Sea Lion");
		_mammalsListModel.Add("Sun Bear");
		_mammalsListModel.Add("Tapir");
		_mammalsListModel.Add("Tasmanian Devil");
		_mammalsListModel.Add("Two-Toed Sloth");
		_mammalsListModel.Add("Wallaby");
		_mammalsListModel.Add("Walrus");
		_mammalsListModel.Add("Warthog");
		_mammalsListModel.Add("Water Buffalo");
		_mammalsListModel.Add("Weddell Seal");
		_mammalsListModel.Add("Western Lowland Gorilla");
		_mammalsListModel.Add("White Rhinoceros");
		_mammalsListModel.Add("White-Tailed Deer");
		_mammalsListModel.Add("Wildebeest");
		_mammalsListModel.Add("Wolf");
		_mammalsListModel.Add("Wolverine");
		_mammalsListModel.Add("Zebra");

		_prehistoricsListModel = new DefaultBitListModel();
		_prehistoricsListModel.Add("Ammonites");
		_prehistoricsListModel.Add("Brachychampsa Montana");
		_prehistoricsListModel.Add("Cretoxyrhina Mantelli");
		_prehistoricsListModel.Add("Devil Frog");
		_prehistoricsListModel.Add("Dolichorhynchops Osborni");
		_prehistoricsListModel.Add("Henodus Chelyops");
		_prehistoricsListModel.Add("Hesperornis Regalis");
		_prehistoricsListModel.Add("Platecarpus");
		_prehistoricsListModel.Add("Protosphyraena");
		_prehistoricsListModel.Add("Protostega Gigas");
		_prehistoricsListModel.Add("Styxosaurus Snowii");
		_prehistoricsListModel.Add("Triceratops Horridus");
		_prehistoricsListModel.Add("Troodon Formosus");
		_prehistoricsListModel.Add("Tusotheuthis Longa");
		_prehistoricsListModel.Add("Tylosaurus Proriger");
		_prehistoricsListModel.Add("Xiphactinus Audax");

		_repitilesListModel = new DefaultBitListModel();
		_repitilesListModel.Add("Alligator Snapping Turtle");
		_repitilesListModel.Add("American Alligator");
		_repitilesListModel.Add("American Crocodile");
		_repitilesListModel.Add("Black Mamba");
		_repitilesListModel.Add("Boa Constrictor");
		_repitilesListModel.Add("Burmese Python");
		_repitilesListModel.Add("Eastern Coral Snake");
		_repitilesListModel.Add("Flying Snake");
		_repitilesListModel.Add("Frilled Lizard");
		_repitilesListModel.Add("Galápagos Tortoise");
		_repitilesListModel.Add("Gavial (Gharial)");
		_repitilesListModel.Add("Gila Monster");
		_repitilesListModel.Add("Green Anaconda");
		_repitilesListModel.Add("Green Basilisk Lizard");
		_repitilesListModel.Add("Green Iguana");
		_repitilesListModel.Add("Green Sea Turtle");
		_repitilesListModel.Add("Hawksbill Sea Turtle");
		_repitilesListModel.Add("Horned Toad");
		_repitilesListModel.Add("Kemp's Ridley Sea Turtle");
		_repitilesListModel.Add("King Cobra");
		_repitilesListModel.Add("Komodo Dragon");
		_repitilesListModel.Add("Leatherback Sea Turtle");
		_repitilesListModel.Add("Loggerhead Sea Turtle");
		_repitilesListModel.Add("Marine Iguana");
		_repitilesListModel.Add("Meller's Chameleon");
		_repitilesListModel.Add("Nile Crocodile");
		_repitilesListModel.Add("Saltwater Crocodile");
		_repitilesListModel.Add("Web-Footed Gecko");
	}

	#endregion
}