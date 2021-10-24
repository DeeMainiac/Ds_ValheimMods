using BepInEx;
using Jotunn.Entities;
using Jotunn.Managers;
using Jotunn.Utils;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using Logger = Jotunn.Logger;

namespace DsDinos
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    [BepInDependency(Jotunn.Main.ModGuid)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.Patch)]
    internal class DsDinos : BaseUnityPlugin
    {
        public const string PluginGUID = "deemainiac.dsdinosaurs";
        public const string PluginName = "D's Dinosaurs";
        public const string PluginVersion = "0.0.1";

        // Use this class to add your own localization to the game
        // https://valheim-modding.github.io/Jotunn/tutorials/localization.html
        //  public static CustomLocalization Localization;

        private AssetBundle staffBundle;

        internal static Harmony _harmony;

        // /Dinosaurs

        /// <summary>
        /// (stygi - moloch) hateful sacrifice.
        /// Thought to be a juvenile Patchycephalosaurus, this dome-headed herbivore primarily consumes fruits, leaves, and seeds.
        /// </summary>
        private static GameObject stygi;

        //   private string SpriteLocations = "";

        private void Awake() {

            LoadAssets();

            PrefabManager.OnVanillaPrefabsAvailable += ApplyDinoStuff;

            _harmony = new Harmony(Info.Metadata.GUID);
            _harmony.PatchAll();

        }

        private void LoadAssets() {

           
                staffBundle = AssetUtils.LoadAssetBundleFromResources("staff", typeof(DsDinos).Assembly);

                stygi = staffBundle.LoadAsset<GameObject>("dd_stygi");

                PrefabManager.Instance.AddPrefab(stygi);
        }

        private void ApplyDinoStuff() {

            try {

                MonsterAI _monsterAI = stygi.GetComponent<MonsterAI>();
            //    Tameable _tameable = stygi.GetComponent<Tameable>();
           //     Humanoid _humanoid = stygi.GetComponent<Humanoid>();
                CharacterDrop _drop = stygi.GetComponent<CharacterDrop>();

                _monsterAI.m_consumeItems = new List<ItemDrop> {
                    ReturnItemDrop(RetrieveGO("Dandelion")),
                    ReturnItemDrop(RetrieveGO("Blueberries")),
                    ReturnItemDrop(RetrieveGO("Raspberry")),
                    ReturnItemDrop(RetrieveGO("Acorn")),
                    ReturnItemDrop(RetrieveGO("AncientSeed")),
                    ReturnItemDrop(RetrieveGO("Cloudberry"))
                };

                _drop.m_drops.Add(new CharacterDrop.Drop {

                    m_prefab = RetrieveGO("LeatherScraps"),
                    m_chance = 1,
                    m_amountMin = 1,
                    m_amountMax = 3,
                    m_levelMultiplier = true,
                    m_onePerPlayer = true
                });

                _drop.m_drops.Add(new CharacterDrop.Drop {

                    m_prefab = RetrieveGO("RawMeat"),
                    m_chance = 1,
                    m_amountMin = 1,
                    m_amountMax = 2,
                    m_levelMultiplier = true,
                    m_onePerPlayer = true
                });

            }
            catch (Exception ex) {
                Logger.LogError($"Error while adding dino stuff: {ex.Message}");
            }
            finally {

                PrefabManager.OnVanillaPrefabsAvailable -= ApplyDinoStuff;
            }
        }



        // / the following borrowed from: https://github.com/sbtoonz/chickenboo/blob/master/ChickenBoo/ChickenBoo.cs \ \\

        private ItemDrop ReturnItemDrop(GameObject gameObject) {
            var drop = gameObject.GetComponent<ItemDrop>();

            return drop;
        }

        private GameObject RetrieveGO(string name) {
            var fab = PrefabManager.Instance.GetPrefab(name);
            return fab;
        }
    }
}