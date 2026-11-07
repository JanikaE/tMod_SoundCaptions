using System;
using System.Collections.Generic;
using System.Globalization;
using SoundCaptions.Common.Audio;
using Terraria.Audio;
using Terraria.ID;

namespace SoundCaptions.Common.Captions;

/// <summary>维护公开声音样式、原版资源路径及部分编号声音族的字幕名称。</summary>
internal static class VanillaSoundNames
{
    private const string SoundRoot = "Terraria/Sounds/";
    private static readonly Dictionary<VolumeTargetKey, string> Names = CreateNames();
    private static readonly Dictionary<string, string> ResourceNames = CreateResourceNames();

    /// <summary>按精确样式、明确资源路径和编号声音族的顺序查询原版文案。</summary>
    internal static bool TryGetName(VolumeTargetKey target, out string name)
    {
        if (Names.TryGetValue(target, out name) ||
            (target.BasePath != null && ResourceNames.TryGetValue(target.BasePath, out name)))
            return true;

        // 编号声音族可共用语义文案，稳定音量目标仍由调用方传入的声音键区分。
        if (IsNumberedFamily(target, SoundRoot + "NPC_Hit_", SoundID.NPCHitCount))
            name = "NpcHurt";
        else if (IsNumberedFamily(target, SoundRoot + "NPC_Killed_", SoundID.NPCDeathCount))
            name = "NpcDeath";

        return name != null;
    }

    /// <summary>判断目标是否属于有效的原版编号声音族或该族的变体集合。</summary>
    private static bool IsNumberedFamily(VolumeTargetKey target, string family, int count)
    {
        if (target.Identifier != null || target.BasePath?.StartsWith(family, StringComparison.Ordinal) != true)
            return false;

        ReadOnlySpan<char> suffix = target.BasePath.AsSpan(family.Length);
        if (suffix.IsEmpty)
            return target.VariantFamily != null;

        return int.TryParse(suffix, NumberStyles.None, CultureInfo.InvariantCulture, out int id) &&
            id > 0 && id < count;
    }

    /// <summary>建立以完整稳定目标键索引的原版声音样式映射。</summary>
    private static Dictionary<VolumeTargetKey, string> CreateNames()
    {
        var names = new Dictionary<VolumeTargetKey, string>();
        // 精确键保留标识符和变体边界，避免共用资源的不同样式被当作同一个音量目标。
        void Add(string name, params SoundStyle[] styles)
        {
            foreach (SoundStyle style in styles)
                names.Add(VolumeTargetKey.FromStyle(style), name);
        }

        Add("DoorOpen", SoundID.DoorOpen);
        Add("DoorClosed", SoundID.DoorClosed);
        Add("Unlock", SoundID.Unlock);
        Add("Pickup", SoundID.Grab);
        Add("CoinPickup", SoundID.CoinPickup);
        Add("Coins", SoundID.Coins);
        Add("Dig", SoundID.Dig);
        Add("Tink", SoundID.Tink);
        Add("Grass", SoundID.Grass);
        Add("Swing", SoundID.Item1, SoundID.DD2_MonkStaffSwing, SoundID.DD2_SkyDragonsFurySwing, SoundID.Item18,
            SoundID.Item19);
        Add("UseItem", SoundID.Item2);
        Add("Drink", SoundID.Item3);
        Add("Chime", SoundID.Item4);
        Add("Bow", SoundID.Item5, SoundID.Item97, SoundID.Item102);
        Add("Gunshot", SoundID.Item11, SoundID.Item36, SoundID.Item41, SoundID.Item31, SoundID.Item38,
            SoundID.Item40, SoundID.Item98, SoundID.Item99);
        Add("Explosion", SoundID.Item14, SoundID.DD2_ExplosiveTrapExplode, SoundID.DD2_KoboldExplosion,
            SoundID.Item62);
        Add("Shatter", SoundID.Shatter);
        Add("PlayerHurt", SoundID.PlayerHit, SoundID.FemaleHit, SoundID.DSTFemaleHurt, SoundID.DSTMaleHurt);
        Add("PlayerKilled", SoundID.PlayerKilled);
        Add("Drown", SoundID.Drown);
        Add("DoubleJump", SoundID.DoubleJump);
        Add("Run", SoundID.Run);
        Add("MaxMana", SoundID.MaxMana);
        Add("Mech", SoundID.Mech);
        Add("MenuOpen", SoundID.MenuOpen);
        Add("MenuClose", SoundID.MenuClose);
        Add("MenuTick", SoundID.MenuTick);
        Add("Chat", SoundID.Chat);
        Add("Camera", SoundID.Camera);
        Add("Achievement", SoundID.AchievementComplete);
        Add("Research", SoundID.Research);
        Add("ResearchComplete", SoundID.ResearchComplete);
        Add("Splash", SoundID.Splash);
        Add("LiquidSplash", SoundID.SplashWeak);
        Add("ShimmerSplash", SoundID.Shimmer1, SoundID.ShimmerWeak2);
        Add("ShimmerEnds", SoundID.Shimmer2);
        Add("Aetherium", SoundID.ShimmerWeak1);
        Add("Drip", SoundID.Drip, SoundID.DripSplash);
        Add("Waterfall", SoundID.Waterfall);
        Add("Lavafall", SoundID.Lavafall);
        Add("LiquidMix", SoundID.LiquidsHoneyLava, SoundID.LiquidsHoneyWater, SoundID.LiquidsWaterLava);
        Add("Thunder", SoundID.Thunder);
        Add("Blizzard", SoundID.BlizzardStrongLoop, SoundID.BlizzardInsideBuildingLoop);
        Add("ZombieMoan", SoundID.ZombieMoan, SoundID.Zombie1, SoundID.Zombie2);
        Add("Roar", SoundID.Roar, SoundID.DD2_OgreRoar, SoundID.DD2_BetsyScream, SoundID.DD2_WyvernScream);
        Add("WormDig", SoundID.WormDig);
        Add("Scream", SoundID.ScaryScream, SoundID.DD2_GoblinScream, SoundID.DD2_GoblinBomberScream,
            SoundID.DD2_KoboldFlyerChargeScream, SoundID.DD2_KoboldScreamChargeLoop);
        Add("Bird", SoundID.Bird, SoundID.Zombie14, SoundID.Zombie16, SoundID.Zombie17, SoundID.Zombie18,
            SoundID.Zombie19);
        Add("Duck", SoundID.Duck, SoundID.Zombie10, SoundID.Zombie11, SoundID.Zombie12);
        Add("Frog", SoundID.Frog);
        Add("Critter", SoundID.Critter);
        Add("Dolphin", SoundID.Dolphin);
        Add("Seagull", SoundID.Seagull, SoundID.Zombie106, SoundID.Zombie107, SoundID.Zombie108);
        Add("Owl", SoundID.Owl, SoundID.Zombie110, SoundID.Zombie111, SoundID.Zombie112, SoundID.Zombie113,
            SoundID.Zombie114);
        Add("Cockatiel", SoundID.Cockatiel, SoundID.Zombie118, SoundID.Zombie119, SoundID.Zombie120);
        Add("Macaw", SoundID.Macaw, SoundID.Zombie126, SoundID.Zombie127, SoundID.Zombie128);
        Add("Toucan", SoundID.Toucan, SoundID.Zombie129, SoundID.Zombie130);
        Add("Pixie", SoundID.Pixie);
        Add("Mummy", SoundID.Mummy, SoundID.Zombie3, SoundID.Zombie4);
        Add("BloodZombie", SoundID.BloodZombie, SoundID.Zombie21, SoundID.Zombie22, SoundID.Zombie23);
        Add("SandShark", SoundID.SandShark);
        Add("Clown", SoundID.Clown, SoundID.Zombie121, SoundID.Zombie122, SoundID.Zombie123);
        Add("QueenSlime", SoundID.QueenSlime, SoundID.Item154, SoundID.Item155, SoundID.Item167,
            SoundID.Zombie115, SoundID.Zombie116, SoundID.Zombie117);
        Add("Meowmere", SoundID.Meowmere, SoundID.Item57, SoundID.Item58);
        Add("AbigailAttack", SoundID.AbigailAttack);
        Add("AbigailCry", SoundID.AbigailCry);
        Add("AbigailSummon", SoundID.AbigailSummon);
        Add("AbigailUpgrade", SoundID.AbigailUpgrade);
        Add("ChesterOpen", SoundID.ChesterOpen);
        Add("ChesterClose", SoundID.ChesterClose);
        Add("LucyTalk", SoundID.LucyTheAxeTalk);
        Add("DeerclopsDeath", SoundID.DeerclopsDeath);
        Add("DeerclopsHit", SoundID.DeerclopsHit);
        Add("DeerclopsIce", SoundID.DeerclopsIceAttack);
        Add("DeerclopsRubble", SoundID.DeerclopsRubbleAttack);
        Add("DeerclopsRoar", SoundID.DeerclopsScream);
        Add("DeerclopsStep", SoundID.DeerclopsStep);
        Add("Guitar", SoundID.GuitarAm, SoundID.GuitarBm, SoundID.GuitarC, SoundID.GuitarD, SoundID.GuitarEm,
            SoundID.GuitarG, SoundID.Item47, SoundID.Item133, SoundID.Item134, SoundID.Item135, SoundID.Item136,
            SoundID.Item137, SoundID.Item138);
        Add("Drum", SoundID.DrumClosedHiHat, SoundID.DrumCymbal1, SoundID.DrumCymbal2, SoundID.DrumFloorTom,
            SoundID.DrumHiHat, SoundID.DrumKick, SoundID.DrumTamaSnare, SoundID.DrumTomHigh, SoundID.DrumTomLow,
            SoundID.DrumTomMid, SoundID.Item139, SoundID.Item140, SoundID.Item141, SoundID.Item142,
            SoundID.Item143, SoundID.Item144, SoundID.Item145, SoundID.Item146, SoundID.Item147, SoundID.Item148);
        Add("FireballShot", SoundID.DD2_BetsyFireballShot);
        Add("FireballImpact", SoundID.DD2_BetsyFireballImpact, SoundID.Item45);
        Add("SharedImpact", SoundID.NPCDeath14, SoundID.Item107, SoundID.NPCDeath2);
        Add("NpcHurt", SoundID.DD2_BetsyHurt, SoundID.DD2_DarkMageHurt, SoundID.DD2_DrakinHurt,
            SoundID.DD2_GoblinBomberHurt, SoundID.DD2_GoblinHurt, SoundID.DD2_JavelinThrowersHurt,
            SoundID.DD2_KoboldFlyerHurt, SoundID.DD2_KoboldHurt, SoundID.DD2_LightningBugHurt,
            SoundID.DD2_OgreHurt, SoundID.DD2_SkeletonHurt, SoundID.DD2_WitherBeastHurt,
            SoundID.DD2_WyvernHurt);
        Add("NpcDeath", SoundID.DD2_BetsyDeath, SoundID.DD2_DarkMageDeath, SoundID.DD2_DrakinDeath,
            SoundID.DD2_GoblinBomberDeath, SoundID.DD2_GoblinDeath, SoundID.DD2_JavelinThrowersDeath,
            SoundID.DD2_KoboldDeath, SoundID.DD2_KoboldFlyerDeath, SoundID.DD2_LightningBugDeath,
            SoundID.DD2_OgreDeath, SoundID.DD2_SkeletonDeath, SoundID.DD2_WitherBeastDeath,
            SoundID.DD2_WyvernDeath);

        Add("AmmoBox", SoundID.Item149);
        Add("AntlionSound", SoundID.Zombie44, SoundID.Zombie45, SoundID.Zombie46);
        Add("BeeBuzz", SoundID.Item173);
        Add("Bell", SoundID.Item35);
        Add("BladeSwing", SoundID.Item71, SoundID.Item116, SoundID.Item169);
        Add("BloodCrawler", SoundID.Zombie24, SoundID.Zombie25);
        Add("Boost", SoundID.Item13, SoundID.Item24);
        Add("BoulderImpact", SoundID.Item70);
        Add("BubblePop", SoundID.Item54);
        Add("ChatteringTeeth", SoundID.Zombie124);
        Add("CultistArrival", SoundID.Zombie105);
        Add("CultistSound", SoundID.Zombie88, SoundID.Zombie89, SoundID.Zombie90, SoundID.Zombie91);
        Add("DartLaunch", SoundID.Item63);
        Add("Deflect", SoundID.Item150);
        Add("DemonSound", SoundID.Zombie26, SoundID.Zombie27, SoundID.Zombie28, SoundID.Zombie29);
        Add("DungeonBrick", SoundID.Item127);
        Add("EmpressSound", SoundID.Item159, SoundID.Item160, SoundID.Item161, SoundID.Item162,
            SoundID.Item163, SoundID.Item164, SoundID.Item165);
        Add("FaceMonster", SoundID.Zombie8);
        Add("Fart", SoundID.Item16);
        Add("FireworksLaunch", SoundID.Item156);
        Add("FishronSound", SoundID.Zombie20);
        Add("Flute", SoundID.Item151);
        Add("Frankenstein", SoundID.Zombie6);
        Add("GolfHole", SoundID.Item129);
        Add("GolfSwing", SoundID.Item126);
        Add("GolfWhistle", SoundID.Item128);
        Add("Harp", SoundID.Item26);
        Add("HitSound", SoundID.NPCHit2);
        Add("IceChink", SoundID.Item27);
        Add("IceImpact", SoundID.Item50);
        Add("IceMist", SoundID.Item120);
        Add("IcePlaced", SoundID.Item30);
        Add("JellyfishSound", SoundID.Zombie34, SoundID.Zombie35);
        Add("LaserFire", SoundID.Item12, SoundID.Item33, SoundID.Item91, SoundID.Item157,
            SoundID.Item158);
        Add("LightningCast", SoundID.Item121, SoundID.Item123);
        Add("MagicImpact", SoundID.Item74, SoundID.Item94, SoundID.Item110, SoundID.Item118);
        Add("MagicPulse", SoundID.Item8, SoundID.Item20, SoundID.Item28, SoundID.Item43,
            SoundID.Item60, SoundID.Item75, SoundID.Item82, SoundID.Item117,
            SoundID.Item92);
        Add("MagicWeapon", SoundID.Item15, SoundID.Item21, SoundID.Item66, SoundID.Item67,
            SoundID.Item69, SoundID.Item72, SoundID.Item73, SoundID.Item84,
            SoundID.Item85, SoundID.Item88, SoundID.Item100, SoundID.Item101,
            SoundID.Item103, SoundID.Item105, SoundID.Item109, SoundID.Item111);
        Add("ManaCrystal", SoundID.Item29);
        Add("MartianMachine", SoundID.Zombie66, SoundID.Zombie67, SoundID.Zombie68, SoundID.Zombie69,
            SoundID.Zombie70, SoundID.Zombie71, SoundID.Zombie72);
        Add("MetalRing", SoundID.Item37);
        Add("MeteorImpact", SoundID.Item89);
        Add("MinecartBounce", SoundID.Item56);
        Add("MinecartBrake", SoundID.Item55);
        Add("MinecartContact", SoundID.Item53);
        Add("MoonLordArrival", SoundID.Zombie92);
        Add("MoonLordSound", SoundID.Zombie93, SoundID.Zombie94, SoundID.Zombie95, SoundID.Zombie96,
            SoundID.Zombie97, SoundID.Zombie98, SoundID.Zombie99);
        Add("MusicRecorded", SoundID.Item166);
        Add("NailGun", SoundID.Item108);
        Add("NautilusSound", SoundID.Item170, SoundID.Item171, SoundID.Item172);
        Add("OddSound", SoundID.NPCDeath19, SoundID.NPCDeath59, SoundID.NPCDeath61);
        Add("PanImpact", SoundID.Item178);
        Add("Parrot", SoundID.Zombie78);
        Add("PhantasmalImpact", SoundID.Zombie103);
        Add("PhantasmalRay", SoundID.Zombie104);
        Add("PhantasmalSphere", SoundID.Zombie102);
        Add("PhantasmSummon", SoundID.Item119);
        Add("Pigron", SoundID.Zombie9, SoundID.Zombie38, SoundID.Zombie39, SoundID.Zombie40);
        Add("Pogo", SoundID.Item168);
        Add("PortalGun", SoundID.Item114, SoundID.Item115);
        Add("ProjectileLaunch", SoundID.Item10, SoundID.Item17, SoundID.Item39, SoundID.Item42,
            SoundID.Item61, SoundID.Item64, SoundID.Item106, SoundID.Item124,
            SoundID.Item125,
            SoundID.Item9, SoundID.Item96, SoundID.Item174);
        Add("QueenBeeSound", SoundID.Zombie125);
        Add("ShieldBreak", SoundID.NPCDeath58);
        Add("ShimmerSound", SoundID.Item176);
        Add("Slap", SoundID.Item175);
        Add("SnowballImpact", SoundID.Item51);
        Add("SnowImpact", SoundID.Item48, SoundID.Item49);
        Add("SpiritSound", SoundID.Zombie53, SoundID.Zombie54, SoundID.Zombie81, SoundID.Zombie82,
            SoundID.Zombie83);
        Add("Spray", SoundID.Item34);
        Add("Squish", SoundID.Item177);
        Add("Summon", SoundID.Item25, SoundID.Item44, SoundID.Item46, SoundID.Item59,
            SoundID.Item76, SoundID.Item77, SoundID.Item78, SoundID.Item79,
            SoundID.Item80, SoundID.Item81, SoundID.Item83, SoundID.Item113);
        Add("TargetHit", SoundID.NPCHit15, SoundID.NPCHit16, SoundID.NPCHit17);
        Add("Teleport", SoundID.Item6);
        Add("ToolMotor", SoundID.Item23, SoundID.Item132);
        Add("TrackImpact", SoundID.Item52);
        Add("TrueEyeSound", SoundID.Zombie100, SoundID.Zombie101);
        Add("VoidBag", SoundID.Item130);
        Add("WeaponDischarge", SoundID.Item7);
        Add("Whip", SoundID.Item152, SoundID.Item153);

        Add("BallistaShot", SoundID.DD2_BallistaTowerShot);
        Add("BetsyBreath", SoundID.DD2_BetsyFlameBreath);
        Add("BetsyCircle", SoundID.DD2_BetsyFlyingCircleAttack);
        Add("BetsyArrival", SoundID.DD2_BetsySummon);
        Add("BetsyWind", SoundID.DD2_BetsyWindAttack);
        Add("BookCast", SoundID.DD2_BookStaffCast);
        Add("BookTwister", SoundID.DD2_BookStaffTwisterLoop);
        Add("EterniaCrystalHit", SoundID.DD2_CrystalCartImpact);
        Add("DarkMageAttack", SoundID.DD2_DarkMageAttack);
        Add("DarkMageHeal", SoundID.DD2_DarkMageCastHeal);
        Add("DarkSigilImpact", SoundID.DD2_DarkMageHealImpact);
        Add("OldOnesSkeleton", SoundID.DD2_DarkMageSummonSkeleton, SoundID.DD2_SkeletonSummoned);
        Add("OldOnesDefeat", SoundID.DD2_DefeatScene);
        Add("DefenseSummon", SoundID.DD2_DefenseTowerSpawn);
        Add("DrakinInhale", SoundID.DD2_DrakinBreathIn);
        Add("DrakinShot", SoundID.DD2_DrakinShot);
        Add("EterniaPortalHum", SoundID.DD2_EtherianPortalIdleLoop);
        Add("EterniaPortalOpen", SoundID.DD2_EtherianPortalOpen);
        Add("EterniaPortalSpawn", SoundID.DD2_EtherianPortalSpawnEnemy);
        Add("Flameburst", SoundID.DD2_FlameburstTowerShot);
        Add("GlaiveGhost", SoundID.DD2_GhastlyGlaiveImpactGhost);
        Add("GlaivePierce", SoundID.DD2_GhastlyGlaivePierce);
        Add("GoblinBombThrow", SoundID.DD2_GoblinBomberThrow);
        Add("JavelinThrow", SoundID.DD2_JavelinThrowersAttack);
        Add("JavelinTaunt", SoundID.DD2_JavelinThrowersTaunt);
        Add("KoboldIgnite", SoundID.DD2_KoboldIgnite);
        Add("KoboldFuse", SoundID.DD2_KoboldIgniteLoop);
        Add("LightningZap", SoundID.DD2_LightningAuraZap, SoundID.DD2_LightningBugZap);
        Add("MonkStaffImpact", SoundID.DD2_MonkStaffGroundImpact);
        Add("MonkStaffMiss", SoundID.DD2_MonkStaffGroundMiss);
        Add("OgreAttack", SoundID.DD2_OgreAttack);
        Add("OgrePound", SoundID.DD2_OgreGroundPound);
        Add("OgreSpit", SoundID.DD2_OgreSpit);
        Add("PhantomPhoenixShot", SoundID.DD2_PhantomPhoenixShot);
        Add("DragonFurySpin", SoundID.DD2_SkyDragonsFuryCircle);
        Add("DragonFuryShot", SoundID.DD2_SkyDragonsFuryShot);
        Add("SonicSlash", SoundID.DD2_SonicBoomBladeSlash);
        Add("OldOnesVictory", SoundID.DD2_WinScene);
        Add("WitherPulse", SoundID.DD2_WitherBeastAuraPulse);
        Add("WitherCrystalHit", SoundID.DD2_WitherBeastCrystalImpact);
        Add("WyvernDive", SoundID.DD2_WyvernDiveDown);
        Add("GlommerBounce", SoundID.GlommerBounce);
        Add("DroneFlight", SoundID.JimsDrone);
        Add("Wingbeat", SoundID.Item32);
        Add("ScramblerSummon", SoundID.Item90);
        Add("XenopopperFire", SoundID.Item95);
        Add("DerplingSound", SoundID.Zombie30, SoundID.Zombie31);
        Add("FlyingSnakeSound", SoundID.Zombie32);
        Add("TortoiseSound", SoundID.Zombie33);
        Add("LihzahrdSound", SoundID.Zombie36, SoundID.Zombie37);
        Add("NimbusSound", SoundID.Zombie41, SoundID.Zombie42, SoundID.Zombie43);
        Add("CrawdadSound", SoundID.Zombie47);
        Add("DeadlySphereSound", SoundID.Zombie48, SoundID.Zombie49);
        Add("ManFlySound", SoundID.Zombie50, SoundID.Zombie51, SoundID.Zombie52);
        Add("GhoulSound", SoundID.Zombie55, SoundID.Zombie56);
        Add("ShellySound", SoundID.Zombie57, SoundID.Zombie58);
        Add("GigazapperSound", SoundID.Zombie59, SoundID.Zombie60);
        Add("GoblinWarlockSound", SoundID.Zombie61, SoundID.Zombie62);
        Add("GraniteGolemSound", SoundID.Zombie63, SoundID.Zombie64, SoundID.Zombie65);
        Add("MothronSound", SoundID.Zombie73);
        Add("MushiSound", SoundID.Zombie74, SoundID.Zombie75, SoundID.Zombie76, SoundID.Zombie77);
        Add("PossessedSound", SoundID.Zombie79, SoundID.Zombie80);
        Add("SalamanderSound", SoundID.Zombie84, SoundID.Zombie85);
        Add("ScutlixSound", SoundID.Zombie86);
        return names;
    }

    /// <summary>建立无公开别名且已确认用途的原版资源路径映射。</summary>
    private static Dictionary<string, string> CreateResourceNames()
    {
        var names = new Dictionary<string, string>(StringComparer.Ordinal);
        // 补充无公开别名的明确路径，解析时让位给精确样式，只影响显示名称。
        void Add(string name, params string[] paths)
        {
            foreach (string path in paths)
                names.Add(SoundRoot + path, name);
        }

        Add("BestReforge", "Custom/best_reforge");
        Add("BarrelFire", "Custom/deadcells_barrel_launcher_fire");
        Add("Explosion", "Custom/deadcells_barrel_launcher_explode", "Custom/deadcells_mushroom_explode");
        Add("FlintCharge", "Custom/deadcells_flint_charge");
        Add("FlintRelease", "Custom/deadcells_flint_release");
        Add("FlintWave", "Custom/deadcells_flint_wave");
        Add("MushroomSummon", "Custom/deadcells_mushroom_summon");
        Add("EaterDig", "Custom/eow_dig");
        Add("FishingSplash", "Custom/fish_splash");
        Add("FuseBurn", "Custom/fuse");
        Add("Hunger", "Custom/hungry");
        Add("Thunder", "Custom/instant_thunder_", "Custom/instant_thunder_0", "Custom/instant_thunder_1",
            "Custom/instant_thunder_2");
        Add("MenuAccept", "Custom/menu_accept");
        Add("MeteorShower", "Custom/meteor_shower_", "Custom/meteor_shower_0", "Custom/meteor_shower_1",
            "Custom/meteor_shower_2", "Custom/meteor_shower_3",
            "Custom/meteor_shower_4", "Custom/meteor_shower_5");
        Add("CattivaSound", "Custom/pal_cattiva", "Custom/pal_cattiva_joy");
        Add("CattivaDistress", "Custom/pal_cattiva_pain");
        Add("ChilletSound", "Custom/pal_chillet", "Custom/pal_chillet_joy");
        Add("ChilletAttack", "Custom/pal_chillet_attack");
        Add("DigtoiseSound", "Custom/pal_digtoise", "Custom/pal_digtoise_joy");
        Add("FoxparksSound", "Custom/pal_foxparks", "Custom/pal_foxparks_joy");
        Add("FoxparksAttack", "Custom/pal_foxparks_attack");
        Add("FoxparksDistress", "Custom/pal_foxparks_pain");
        Add("PalSummon", "Custom/pal_summon");
        Add("PlayerKilled", "Custom/player_death_retro");
        Add("PlayerHurt", "Custom/player_hit_balloon", "Custom/player_hit_cat",
            "Custom/player_hit_chicken_rare", "Custom/player_hit_chicken",
            "Custom/player_hit_crow", "Custom/player_hit_default",
            "Custom/player_hit_dog", "Custom/player_hit_fairy",
            "Custom/player_hit_frog", "Custom/player_hit_goat",
            "Custom/player_hit_goblin", "Custom/player_hit_retro",
            "Custom/player_hit_turkey", "Custom/player_hit_undead",
            "Custom/player_hit_vampire");
        Add("RainbowChime", "Custom/rainbow_boulder");
        Add("RemoteCar", "Custom/rccar");
        Add("SunlightBurn", "Custom/sizzle");
        Add("Sonar", "Custom/sonar_potion");
        Add("StatueJump", "Custom/statuemimic_jump");
        Add("StatueLaugh", "Custom/statuemimic_laugh_", "Custom/statuemimic_laugh_0", "Custom/statuemimic_laugh_1",
            "Custom/statuemimic_laugh_2");
        Add("StatueScare", "Custom/statuemimic_scare");
        Add("TrashItem", "Custom/trash_item_", "Custom/trash_item_0", "Custom/trash_item_1");
        Add("AlchemyFlask", "Item_179");
        Add("WeaponDischarge", "Item_180");
        Add("Spray", "Item_181");
        Add("BladeSwing", "Item_182");
        Add("Gunshot", "Item_183", "Item_184");
        Add("NightAmulet", "Item_185");
        Add("SlasherSkull", "Item_186");
        Add("Flute", "Item_187");
        Add("WolfNecklace", "Item_188");
        Add("ToolMotor", "Item_189", "Item_190");
        Add("KiteReel", "Item_192");
        Add("EelSound", "Item_193");
        Add("StickUnicorn", "Item_196");
        Add("SpectateOrb", "Item_197", "Item_198");
        Add("IceGunshot", "Item_201");
        Add("ElectricArc", "Item_203");
        Add("LightningHit", "Item_204");
        Add("MagicPulse", "Item_205");
        Add("VolcanicStone", "Item_206");
        Add("ArmletSound", "Item_207");
        Add("IceCharge", "Item_210");
        Add("StatueHit", "NPC_Hit_58");
        Add("StatueDeath", "NPC_Killed_67");
        Add("DigtoiseDrill", "Item_199");
        return names;
    }
}
