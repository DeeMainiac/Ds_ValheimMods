using BepInEx;
using BepInEx.Configuration;
using Jotunn.Configs;
using Jotunn.Entities;
using Jotunn.GUI;
using Jotunn.Managers;
using Jotunn.Utils;
using Jotunn.ConsoleCommands;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Jotunn.Logger;

/// <summary>
/// Seafloor Walking Boots
/// 
/// A Valheim mod for walking underwater with special boots
/// 
/// By: DeeMainiac
/// 
/// Boots Icon by Lorc under CC BY 3.0
/// https://game-icons.net/1x1/lorc/boots.html
/// 
/// </summary>
namespace SeafloorWalkingBoots {
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Major)]
    
    internal class SeafloorWalkingBoots : BaseUnityPlugin {
        public const string PluginGUID = "deemainiac.seafloorwalkingboots";
        public const string PluginName = "SeafloorWalkingBoots";
        public const string PluginVersion = "1.0.0";

        public static CustomLocalization Localization;

        private ItemDrop lozIronBoots;
        CustomStatusEffect bootsEffect;
        
        // /hardcoded place where the image file is. Make sure to change this if you copy the code
        private readonly string SpriteLocations = "";

        private void Awake() {

            AddStatusEffects();

            AddLocalizations();

            PrefabManager.OnVanillaPrefabsAvailable += AddClonedItems;

            Logger.LogInfo("Seafloor Walking Boots has arrived");

            // To learn more about Jotunn's features, go to
            // https://valheim-modding.github.io/Jotunn/tutorials/overview.html
        }
  
        private void AddClonedItems() {

            try {
                WWBoots();
            }
            catch (Exception ex) {
                Logger.LogError($"Error while adding cloned item: {ex.Message}");
            }
            finally {

                PrefabManager.OnVanillaPrefabsAvailable -= AddClonedItems;
            }

        }

        /// <summary>
        ///  Clones similar item from vanilla Valheim and converts it into the seafloor walking boots
        /// </summary>
        private void WWBoots() {

            CustomItem CI = new CustomItem("IronBoots", "ArmorIronLegs");
            ItemManager.Instance.AddItem(CI);

            lozIronBoots = CI.ItemDrop;

            lozIronBoots.m_itemData.m_shared.m_name = "$item_ironboots";
            lozIronBoots.m_itemData.m_shared.m_description = "$item_ironboots_desc";
            lozIronBoots.m_itemData.m_shared.m_armor = 1;
            lozIronBoots.m_itemData.m_shared.m_movementModifier = -.4f;

            lozIronBoots.m_itemData.m_shared.m_equipStatusEffect = bootsEffect.StatusEffect;

            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.name = "Recipe_ironboots";
            recipe.m_item = lozIronBoots;

            // /crafting station to make/repair it at
            recipe.m_craftingStation = PrefabManager.Cache.GetPrefab<CraftingStation>("forge");

            // /crafting recipe for it
            recipe.m_resources = new Piece.Requirement[]{

                new Piece.Requirement() {

                    m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("Iron"),
                    m_amount = 20
            },
            new Piece.Requirement() {

                m_resItem = PrefabManager.Cache.GetPrefab<ItemDrop>("NeckTail"),
                m_amount = 1
                }
            };

            CustomRecipe cr = new CustomRecipe(recipe, fixReference: false, fixRequirementReferences: false);
            ItemManager.Instance.AddRecipe(cr);
        }

        /// <summary>
        /// Adds the water walking effect outlined in SE_boots.cs
        /// </summary>
        private void AddStatusEffects() {

            StatusEffect effect = ScriptableObject.CreateInstance<SE_boots>();

            effect.name = "IronBootsEffect";
            effect.m_name = "$ironboots_effectname";
            effect.m_icon = AssetUtils.LoadSpriteFromFile(SpriteLocations + "boots.png");
            effect.m_startMessageType = MessageHud.MessageType.Center;
            effect.m_startMessage = "$ironboots_effectstart";
            effect.m_stopMessageType = MessageHud.MessageType.Center;
            effect.m_stopMessage = "$ironboots_effectstop";

            bootsEffect = new CustomStatusEffect(effect, fixReference: false);

            ItemManager.Instance.AddStatusEffect(bootsEffect);
        }
        /// <summary>
        /// Adds localizations for this mod.
        /// </summary>
        private void AddLocalizations() {

            Localization = new CustomLocalization();

            LocalizationManager.Instance.AddLocalization(Localization);

            Localization.AddTranslation("English", new Dictionary<string, string> {
                {"item_ironboots","Seafloor Walking Boots"},{"item_ironboots_desc", "So heavy, you can't run. So heavy, you can't float."},
                {"$ironboots_effectstart","You feel heavier"},{"$ironboots_effectstop","You feel lighter"},
                {"$ironboots_effectname","Seafloor Walking"}
            });
        }
    }
}
