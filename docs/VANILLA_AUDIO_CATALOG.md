# 原版音频映射清单

以本机 tModLoader 2026.09.3.1 的程序集和 Terraria 1.4.5.8 音效目录为基线。逐项记录 623 个公开 SoundStyle 字段、607 个独立音量目标、863 个 XNB 音效资源和 88 首注册曲目。此表按当前程序解析结果整理，语义状态与游戏内听测状态分开。[SoundID 参考](https://docs.tmodloader.net/docs/stable/class_sound_i_d.html) · [MusicID 参考](https://docs.tmodloader.net/docs/stable/class_music_i_d.html)

| 字段的命名状态 | 数量 | 判断依据 |
| --- | ---: | --- |
| 准确语义 | 195 | 从命名资源、公开别名、原版调用或已核对的 Wiki 来源确认语义。 |
| 共享通用语义 | 414 | 使用声音或事件类别。多处共用的编号受伤、死亡等音效保留通用描述。 |
| 原版未使用 | 13 | Wiki 标注无当前原版用途。 |
| 资源路径模板 | 1 | `SoundID.Item` 提供路径前缀，具体声音由编号或变体决定。 |

旧版入口 GetLegacyStyle 的类型 0–69、变体 -1 至 ItemSoundCount 范围内，有 499 个有效音量目标，全部已包含在公开字段表。空资源路径排除。旧版入口覆盖的是播放映射，资产表另列没有公开别名的文件。

## 核对依据

Wiki 复核用中英文「声音 ID」的可读索引条目核对来源，并以本机 tModLoader 的 SoundStyle 和资源路径确认映射。公开字段表以当前 tModLoader 程序集为准。Wiki 的 1.4.5 条目用于核对本机实际存在、但当前 tModLoader 没有公开字段的资源。共享资源按共同用途或声音特征命名。标为未使用的资源仍可由第三方主动播放，此时显示通用字幕。[中文声音 ID](https://terraria.wiki.gg/zh/wiki/%E5%A3%B0%E9%9F%B3_ID) · [英文 Sound IDs](https://terraria.wiki.gg/wiki/Sound_IDs)

复核示例包括传送类物品、矿车碰轨与刹车、冰雪受击、挥鞭、训练假人以及天界柱护盾破裂。法力回满与武器冷却共用的音效显示「就绪提示音」，僵尸和骷髅等敌怪共用的低吼显示「低吼声」。同名条目的音量目标仍由 SoundStyle 决定。Wiki 对 Item_199 的用途栏留空，本机 Terraria 1.4.5.8 的 Projectile.AI_204_Digtoise 直接播放此资源，因此标为「碎岩龟钻动」。

## 公开 SoundID

同一资源路径、标识符和音量变体族解析为同一目标。指定标识符时，变体后缀仍决定实际播放资源，音量身份由路径与标识符确定。相同字幕文字可以对应多个独立目标。表中的路径相对于 Terraria/Sounds/。

| SoundID | 资源路径 | 标识符 | 资源后缀 | 命名状态 | 中文字幕 · 键 |
| --- | --- | --- | --- | --- | --- |
| `AbigailAttack` | `Custom/abigail_attack` | — | — | 准确语义 | 阿比盖尔攻击 · `AbigailAttack` |
| `AbigailCry` | `Custom/abigail_cry_` | — | `0,1,2` | 准确语义 | 阿比盖尔哭声 · `AbigailCry` |
| `AbigailSummon` | `Custom/abigail_summon` | — | — | 准确语义 | 召唤阿比盖尔 · `AbigailSummon` |
| `AbigailUpgrade` | `Custom/abigail_upgrade_` | — | `0,1,2` | 准确语义 | 阿比盖尔强化 · `AbigailUpgrade` |
| `AchievementComplete` | `Custom/achievement_complete` | — | — | 准确语义 | 达成成就 · `Achievement` |
| `Bird` | `Zombie_` | `Terraria/Bird` | `14,16,17,18,19` | 共享通用语义 | 鸟鸣 · `Bird` |
| `Bird14` | `Zombie_` | `Terraria/Bird` | `14` | 共享通用语义 | 鸟鸣 · `Bird` |
| `Bird16` | `Zombie_` | `Terraria/Bird` | `16` | 共享通用语义 | 鸟鸣 · `Bird` |
| `Bird17` | `Zombie_` | `Terraria/Bird` | `17` | 共享通用语义 | 鸟鸣 · `Bird` |
| `Bird18` | `Zombie_` | `Terraria/Bird` | `18` | 共享通用语义 | 鸟鸣 · `Bird` |
| `Bird19` | `Zombie_` | `Terraria/Bird` | `19` | 共享通用语义 | 鸟鸣 · `Bird` |
| `BlizzardInsideBuildingLoop` | `Custom/blizzard_inside_building_loop` | — | — | 共享通用语义 | 暴风雪呼啸 · `Blizzard` |
| `BlizzardStrongLoop` | `Custom/blizzard_strong_loop` | — | — | 共享通用语义 | 暴风雪呼啸 · `Blizzard` |
| `BloodZombie` | `Zombie_` | `Terraria/BloodZombie` | `21,22,23` | 共享通用语义 | 血月怪物低吼 · `BloodZombie` |
| `Camera` | `Camera` | — | — | 准确语义 | 快门声 · `Camera` |
| `Chat` | `Chat` | — | — | 准确语义 | 聊天提示 · `Chat` |
| `ChesterClose` | `Custom/chester_close_` | — | `0,1` | 准确语义 | 切斯特关闭 · `ChesterClose` |
| `ChesterOpen` | `Custom/chester_open_` | — | `0,1` | 准确语义 | 切斯特打开 · `ChesterOpen` |
| `Clown` | `Zombie_` | `Terraria/Clown` | `121,122,123` | 准确语义 | 小丑叫声 · `Clown` |
| `Cockatiel` | `Zombie_` | `Terraria/Cockatiel` | `118,119,120` | 准确语义 | 玄凤鹦鹉叫声 · `Cockatiel` |
| `CoinPickup` | `Coin_` | — | `0,1,2,3,4` | 共享通用语义 | 钱币叮当 · `CoinPickup` |
| `Coins` | `Coins` | — | — | 共享通用语义 | 钱币叮当 · `Coins` |
| `Critter` | `Zombie_15` | — | — | 共享通用语义 | 小动物叫声 · `Critter` |
| `DD2_BallistaTowerShot` | `Custom/dd2_ballista_tower_shot_` | — | `0,1,2` | 准确语义 | 弩车发射 · `BallistaShot` |
| `DD2_BetsyDeath` | `Custom/dd2_betsy_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_BetsyFireballImpact` | `Custom/dd2_betsy_fireball_impact_` | — | `0,1,2` | 共享通用语义 | 火球命中 · `FireballImpact` |
| `DD2_BetsyFireballShot` | `Custom/dd2_betsy_fireball_shot_` | — | `0,1,2` | 共享通用语义 | 火球发射 · `FireballShot` |
| `DD2_BetsyFlameBreath` | `Custom/dd2_betsy_flame_breath` | — | — | 准确语义 | 双足翼龙喷火 · `BetsyBreath` |
| `DD2_BetsyFlyingCircleAttack` | `Custom/dd2_betsy_flying_circle_attack` | — | — | 准确语义 | 双足翼龙盘旋攻击 · `BetsyCircle` |
| `DD2_BetsyHurt` | `Custom/dd2_betsy_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_BetsyScream` | `Custom/dd2_betsy_scream` | — | — | 共享通用语义 | 吼叫 · `Roar` |
| `DD2_BetsySummon` | `Custom/dd2_betsy_summon_` | — | `0,1,2` | 准确语义 | 双足翼龙现身 · `BetsyArrival` |
| `DD2_BetsyWindAttack` | `Custom/dd2_betsy_wind_attack_` | — | `0,1,2` | 准确语义 | 双足翼龙掀起狂风 · `BetsyWind` |
| `DD2_BetsysWrathImpact` | `Custom/dd2_betsy_fireball_impact_` | — | `0,1,2` | 共享通用语义 | 火球命中 · `FireballImpact` |
| `DD2_BetsysWrathShot` | `Custom/dd2_betsy_fireball_shot_` | — | `0,1,2` | 共享通用语义 | 火球发射 · `FireballShot` |
| `DD2_BookStaffCast` | `Custom/dd2_book_staff_cast_` | — | `0,1,2` | 准确语义 | 无限智慧巨著施法 · `BookCast` |
| `DD2_BookStaffTwisterLoop` | `Custom/dd2_book_staff_twister_loop` | — | — | 准确语义 | 魔法旋风呼啸 · `BookTwister` |
| `DD2_CrystalCartImpact` | `Custom/dd2_crystal_cart_impact_` | — | `0,1,2` | 准确语义 | 埃特尼亚水晶受击 · `EterniaCrystalHit` |
| `DD2_DarkMageAttack` | `Custom/dd2_dark_mage_attack_` | — | `0,1,2` | 准确语义 | 黑暗魔法师召唤黑暗能量 · `DarkMageAttack` |
| `DD2_DarkMageCastHeal` | `Custom/dd2_dark_mage_cast_heal_` | — | `0,1,2` | 准确语义 | 黑暗魔法师施放治疗 · `DarkMageHeal` |
| `DD2_DarkMageDeath` | `Custom/dd2_dark_mage_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_DarkMageHealImpact` | `Custom/dd2_dark_mage_heal_impact_` | — | `0,1,2` | 准确语义 | 黑暗印记落下 · `DarkSigilImpact` |
| `DD2_DarkMageHurt` | `Custom/dd2_dark_mage_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_DarkMageSummonSkeleton` | `Custom/dd2_dark_mage_summon_skeleton_` | — | `0,1,2` | 共享通用语义 | 撒旦骷髅现身 · `OldOnesSkeleton` |
| `DD2_DefeatScene` | `Custom/dd2_defeat_scene` | — | — | 准确语义 | 撒旦军队防守失败 · `OldOnesDefeat` |
| `DD2_DefenseTowerSpawn` | `Custom/dd2_defense_tower_spawn` | — | — | 共享通用语义 | 召唤防御设施 · `DefenseSummon` |
| `DD2_DrakinBreathIn` | `Custom/dd2_drakin_breath_in_` | — | `0,1,2` | 准确语义 | 德拉克龙吸气 · `DrakinInhale` |
| `DD2_DrakinDeath` | `Custom/dd2_drakin_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_DrakinHurt` | `Custom/dd2_drakin_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_DrakinShot` | `Custom/dd2_drakin_shot_` | — | `0,1,2` | 准确语义 | 德拉克龙吐出投射物 · `DrakinShot` |
| `DD2_EtherianPortalDryadTouch` | `Custom/dd2_etherian_portal_dryad_touch` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `DD2_EtherianPortalIdleLoop` | `Custom/dd2_etherian_portal_idle_loop` | — | — | 准确语义 | 神秘传送门嗡鸣 · `EterniaPortalHum` |
| `DD2_EtherianPortalOpen` | `Custom/dd2_etherian_portal_open` | — | — | 准确语义 | 神秘传送门开启 · `EterniaPortalOpen` |
| `DD2_EtherianPortalSpawnEnemy` | `Custom/dd2_etherian_portal_spawn_enemy_` | — | `0,1,2` | 准确语义 | 敌怪从传送门出现 · `EterniaPortalSpawn` |
| `DD2_ExplosiveTrapExplode` | `Custom/dd2_explosive_trap_explode_` | — | `0,1,2` | 共享通用语义 | 爆炸 · `Explosion` |
| `DD2_FlameburstTowerShot` | `Custom/dd2_flameburst_tower_shot_` | — | `0,1,2` | 准确语义 | 爆炸烈焰塔发射 · `Flameburst` |
| `DD2_GhastlyGlaiveImpactGhost` | `Custom/dd2_ghastly_glaive_impact_ghost_` | — | `0,1,2` | 准确语义 | 长刀召出幽魂 · `GlaiveGhost` |
| `DD2_GhastlyGlaivePierce` | `Custom/dd2_ghastly_glaive_pierce_` | — | `0,1,2` | 准确语义 | 长刀刺击 · `GlaivePierce` |
| `DD2_GoblinBomb` | `NPC_Killed_14` | — | — | 共享通用语义 | 撞击声 · `SharedImpact` |
| `DD2_GoblinBomberDeath` | `Custom/dd2_goblin_bomber_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_GoblinBomberHurt` | `Custom/dd2_goblin_bomber_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_GoblinBomberScream` | `Custom/dd2_goblin_bomber_scream_` | — | `0,1,2` | 共享通用语义 | 尖叫 · `Scream` |
| `DD2_GoblinBomberThrow` | `Custom/dd2_goblin_bomber_throw_` | — | `0,1,2` | 准确语义 | 哥布林投出炸弹 · `GoblinBombThrow` |
| `DD2_GoblinDeath` | `Custom/dd2_goblin_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_GoblinHurt` | `Custom/dd2_goblin_hurt_` | — | `0,1,2,3,4,5` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_GoblinScream` | `Custom/dd2_goblin_scream_` | — | `0,1,2` | 共享通用语义 | 尖叫 · `Scream` |
| `DD2_JavelinThrowersAttack` | `Custom/dd2_javelin_throwers_attack_` | — | `0,1,2` | 准确语义 | 标枪投出 · `JavelinThrow` |
| `DD2_JavelinThrowersDeath` | `Custom/dd2_javelin_throwers_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_JavelinThrowersHurt` | `Custom/dd2_javelin_throwers_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_JavelinThrowersTaunt` | `Custom/dd2_javelin_throwers_taunt_` | — | `0,1,2` | 准确语义 | 标枪投掷者挑衅 · `JavelinTaunt` |
| `DD2_KoboldDeath` | `Custom/dd2_kobold_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_KoboldExplosion` | `Custom/dd2_kobold_explosion_` | — | `0,1,2` | 共享通用语义 | 爆炸 · `Explosion` |
| `DD2_KoboldFlyerChargeScream` | `Custom/dd2_kobold_flyer_charge_scream_` | — | `0,1,2` | 共享通用语义 | 尖叫 · `Scream` |
| `DD2_KoboldFlyerDeath` | `Custom/dd2_kobold_flyer_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_KoboldFlyerHurt` | `Custom/dd2_kobold_flyer_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_KoboldHurt` | `Custom/dd2_kobold_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_KoboldIgnite` | `Custom/dd2_kobold_ignite` | — | — | 准确语义 | 小妖魔点燃炸药 · `KoboldIgnite` |
| `DD2_KoboldIgniteLoop` | `Custom/dd2_kobold_ignite_loop` | — | — | 准确语义 | 小妖魔的引线燃烧 · `KoboldFuse` |
| `DD2_KoboldScreamChargeLoop` | `Custom/dd2_kobold_scream_charge_loop` | — | — | 共享通用语义 | 尖叫 · `Scream` |
| `DD2_LightningAuraZap` | `Custom/dd2_lightning_aura_zap_` | — | `0,1,2,3` | 共享通用语义 | 电击声 · `LightningZap` |
| `DD2_LightningBugDeath` | `Custom/dd2_lightning_bug_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_LightningBugHurt` | `Custom/dd2_lightning_bug_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_LightningBugZap` | `Custom/dd2_lightning_bug_zap_` | — | `0,1,2` | 共享通用语义 | 电击声 · `LightningZap` |
| `DD2_MonkStaffGroundImpact` | `Custom/dd2_monk_staff_ground_impact_` | — | `0,1,2` | 准确语义 | 武器砸地 · `MonkStaffImpact` |
| `DD2_MonkStaffGroundMiss` | `Custom/dd2_monk_staff_ground_miss_` | — | `0,1,2` | 准确语义 | 武器挥空 · `MonkStaffMiss` |
| `DD2_MonkStaffSwing` | `Custom/dd2_monk_staff_swing_` | — | `0,1,2,3` | 共享通用语义 | 挥动 · `Swing` |
| `DD2_OgreAttack` | `Custom/dd2_ogre_attack_` | — | `0,1,2` | 准确语义 | 食人魔攻击 · `OgreAttack` |
| `DD2_OgreDeath` | `Custom/dd2_ogre_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_OgreGroundPound` | `Custom/dd2_ogre_ground_pound` | — | — | 准确语义 | 食人魔猛砸地面 · `OgrePound` |
| `DD2_OgreHurt` | `Custom/dd2_ogre_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_OgreRoar` | `Custom/dd2_ogre_roar_` | — | `0,1,2` | 共享通用语义 | 吼叫 · `Roar` |
| `DD2_OgreSpit` | `Custom/dd2_ogre_spit` | — | — | 准确语义 | 食人魔吐出黏液 · `OgreSpit` |
| `DD2_PhantomPhoenixShot` | `Custom/dd2_phantom_phoenix_shot_` | — | `0,1,2` | 准确语义 | 幽灵凤凰发射 · `PhantomPhoenixShot` |
| `DD2_SkeletonDeath` | `Custom/dd2_skeleton_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_SkeletonHurt` | `Custom/dd2_skeleton_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_SkeletonSummoned` | `Custom/dd2_skeleton_summoned` | — | — | 共享通用语义 | 撒旦骷髅现身 · `OldOnesSkeleton` |
| `DD2_SkyDragonsFuryCircle` | `Custom/dd2_sky_dragons_fury_circle_` | — | `0,1,2` | 准确语义 | 天龙之怒旋转 · `DragonFurySpin` |
| `DD2_SkyDragonsFuryShot` | `Custom/dd2_sky_dragons_fury_shot_` | — | `0,1,2` | 准确语义 | 天龙之怒发射 · `DragonFuryShot` |
| `DD2_SkyDragonsFurySwing` | `Custom/dd2_sky_dragons_fury_swing_` | — | `0,1,2,3` | 共享通用语义 | 挥动 · `Swing` |
| `DD2_SonicBoomBladeSlash` | `Custom/dd2_sonic_boom_blade_slash_` | — | `0,1,2` | 准确语义 | 飞龙挥出音波 · `SonicSlash` |
| `DD2_WinScene` | `Custom/dd2_win_scene` | — | — | 准确语义 | 撒旦军队防守胜利 · `OldOnesVictory` |
| `DD2_WitherBeastAuraPulse` | `Custom/dd2_wither_beast_aura_pulse_` | — | `0,1` | 准确语义 | 枯萎兽光环脉动 · `WitherPulse` |
| `DD2_WitherBeastCrystalImpact` | `Custom/dd2_wither_beast_crystal_impact_` | — | `0,1,2` | 准确语义 | 枯萎兽水晶撞击 · `WitherCrystalHit` |
| `DD2_WitherBeastDeath` | `Custom/dd2_wither_beast_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_WitherBeastHurt` | `Custom/dd2_wither_beast_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_WyvernDeath` | `Custom/dd2_wyvern_death_` | — | `0,1,2` | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `DD2_WyvernDiveDown` | `Custom/dd2_wyvern_dive_down_` | — | `0,1,2` | 准确语义 | 埃特尼亚飞龙俯冲 · `WyvernDive` |
| `DD2_WyvernHurt` | `Custom/dd2_wyvern_hurt_` | — | `0,1,2` | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `DD2_WyvernScream` | `Custom/dd2_wyvern_scream_` | — | `0,1,2` | 共享通用语义 | 吼叫 · `Roar` |
| `DSTFemaleHurt` | `Custom/dst_female_hit_` | — | `0,1,2` | 共享通用语义 | 玩家受伤 · `PlayerHurt` |
| `DSTMaleHurt` | `Custom/dst_male_hit_` | — | `0,1,2` | 共享通用语义 | 玩家受伤 · `PlayerHurt` |
| `DeerclopsDeath` | `Custom/deerclops_death` | — | — | 准确语义 | 独眼巨鹿死亡 · `DeerclopsDeath` |
| `DeerclopsHit` | `Custom/deerclops_hit_` | — | `0,1,2` | 准确语义 | 独眼巨鹿受伤 · `DeerclopsHit` |
| `DeerclopsIceAttack` | `Custom/deerclops_ice_attack_` | — | `0,1,2` | 准确语义 | 独眼巨鹿释放冰刺 · `DeerclopsIce` |
| `DeerclopsRubbleAttack` | `Custom/deerclops_rubble_attack` | — | — | 准确语义 | 独眼巨鹿投掷碎石 · `DeerclopsRubble` |
| `DeerclopsScream` | `Custom/deerclops_scream_` | — | `0,1,2` | 准确语义 | 独眼巨鹿吼叫 · `DeerclopsRoar` |
| `DeerclopsStep` | `Custom/deerclops_step` | — | — | 准确语义 | 独眼巨鹿脚步 · `DeerclopsStep` |
| `Dig` | `Dig_` | — | `0,1,2` | 共享通用语义 | 方块敲击 · `Dig` |
| `Dolphin` | `Zombie_109` | — | — | 准确语义 | 海豚叫声 · `Dolphin` |
| `DoorClosed` | `Door_Closed` | — | — | 准确语义 | 门关闭 · `DoorClosed` |
| `DoorOpen` | `Door_Opened` | — | — | 共享通用语义 | 开门声 · `DoorOpen` |
| `DoubleJump` | `Double_Jump` | — | — | 准确语义 | 二段跳 · `DoubleJump` |
| `Drip` | `Drip_` | — | `0,1` | 共享通用语义 | 滴水声 · `Drip` |
| `DripSplash` | `Drip_2` | — | — | 共享通用语义 | 滴水声 · `Drip` |
| `Drown` | `Drown` | — | — | 准确语义 | 溺水 · `Drown` |
| `DrumClosedHiHat` | `Item_143` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumCymbal1` | `Item_144` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumCymbal2` | `Item_145` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumFloorTom` | `Item_148` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumHiHat` | `Item_139` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumKick` | `Item_146` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumTamaSnare` | `Item_147` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumTomHigh` | `Item_140` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumTomLow` | `Item_141` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `DrumTomMid` | `Item_142` | `Terraria/Drums` | — | 共享通用语义 | 鼓点 · `Drum` |
| `Duck` | `Zombie_` | `Terraria/Duck` | `10,11,12` | 准确语义 | 鸭叫 · `Duck` |
| `FemaleHit` | `Female_Hit_` | — | `0,1,2` | 共享通用语义 | 玩家受伤 · `PlayerHurt` |
| `ForceRoar` | `Roar_0` | `Terraria/Roar` | — | 共享通用语义 | 吼叫 · `Roar` |
| `ForceRoarPitched` | `Roar_0` | `Terraria/Roar` | — | 共享通用语义 | 吼叫 · `Roar` |
| `Frog` | `Zombie_13` | — | — | 准确语义 | 蛙鸣 · `Frog` |
| `GlommerBounce` | `Custom/glommer_bounce_` | — | `0,1` | 准确语义 | 咕噜咪弹跳 · `GlommerBounce` |
| `Grab` | `Grab` | — | — | 共享通用语义 | 物品拾取 · `Pickup` |
| `Grass` | `Grass` | — | — | 共享通用语义 | 草叶沙沙 · `Grass` |
| `GuitarAm` | `Item_138` | `Terraria/Guitar` | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `GuitarBm` | `Item_137` | `Terraria/Guitar` | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `GuitarC` | `Item_133` | `Terraria/Guitar` | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `GuitarD` | `Item_134` | `Terraria/Guitar` | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `GuitarEm` | `Item_135` | `Terraria/Guitar` | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `GuitarG` | `Item_136` | `Terraria/Guitar` | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item` | `Item_` | — | — | 资源路径模板 | 原版音效 · `UnmappedVanilla` |
| `Item1` | `Item_` | — | `1,18,19` | 共享通用语义 | 挥动 · `Swing` |
| `Item10` | `Item_10` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item100` | `Item_100` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item101` | `Item_101` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item102` | `Item_102` | — | — | 共享通用语义 | 弓弦弹响 · `Bow` |
| `Item103` | `Item_103` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item104` | `Item_104` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item105` | `Item_105` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item106` | `Item_106` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item107` | `Item_107` | — | — | 共享通用语义 | 撞击声 · `SharedImpact` |
| `Item108` | `Item_108` | — | — | 准确语义 | 钉枪发射 · `NailGun` |
| `Item109` | `Item_109` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item11` | `Item_11` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item110` | `Item_110` | — | — | 共享通用语义 | 魔法投射物撞击 · `MagicImpact` |
| `Item111` | `Item_111` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item112` | `Item_112` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item113` | `Item_113` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item114` | `Item_114` | — | — | 共享通用语义 | 传送枪发声 · `PortalGun` |
| `Item115` | `Item_115` | — | — | 共享通用语义 | 传送枪发声 · `PortalGun` |
| `Item116` | `Item_116` | — | — | 共享通用语义 | 利刃挥动 · `BladeSwing` |
| `Item117` | `Item_117` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item118` | `Item_118` | — | — | 共享通用语义 | 魔法投射物撞击 · `MagicImpact` |
| `Item119` | `Item_119` | — | — | 准确语义 | 幻影龙现身 · `PhantasmSummon` |
| `Item12` | `Item_12` | — | — | 共享通用语义 | 激光发射 · `LaserFire` |
| `Item120` | `Item_120` | — | — | 准确语义 | 冰雪雾出现 · `IceMist` |
| `Item121` | `Item_121` | — | — | 共享通用语义 | 雷电法术发动 · `LightningCast` |
| `Item122` | `Item_122` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item123` | `Item_123` | — | — | 共享通用语义 | 雷电法术发动 · `LightningCast` |
| `Item124` | `Item_124` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item125` | `Item_125` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item126` | `Item_126` | — | — | 准确语义 | 高尔夫球杆挥动 · `GolfSwing` |
| `Item127` | `Item_127` | — | — | 准确语义 | 地牢砖碎裂 · `DungeonBrick` |
| `Item128` | `Item_128` | — | — | 准确语义 | 高尔夫口哨声 · `GolfWhistle` |
| `Item129` | `Item_129` | — | — | 准确语义 | 高尔夫球进洞 · `GolfHole` |
| `Item13` | `Item_13` | — | — | 共享通用语义 | 推进声 · `Boost` |
| `Item130` | `Item_130` | — | — | 准确语义 | 虚空袋响动 · `VoidBag` |
| `Item131` | `Item_131` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item132` | `Item_132` | — | — | 共享通用语义 | 工具运转声 · `ToolMotor` |
| `Item133` | `Item_133` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item134` | `Item_134` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item135` | `Item_135` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item136` | `Item_136` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item137` | `Item_137` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item138` | `Item_138` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item139` | `Item_139` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item14` | `Item_14` | — | — | 共享通用语义 | 爆炸 · `Explosion` |
| `Item140` | `Item_140` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item141` | `Item_141` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item142` | `Item_142` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item143` | `Item_143` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item144` | `Item_144` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item145` | `Item_145` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item146` | `Item_146` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item147` | `Item_147` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item148` | `Item_148` | — | — | 共享通用语义 | 鼓点 · `Drum` |
| `Item149` | `Item_149` | — | — | 准确语义 | 使用弹药箱 · `AmmoBox` |
| `Item15` | `Item_15` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item150` | `Item_150` | — | — | 准确语义 | 投射物被弹开 · `Deflect` |
| `Item151` | `Item_151` | — | — | 准确语义 | 长笛声 · `Flute` |
| `Item152` | `Item_152` | — | — | 共享通用语义 | 鞭子抽响 · `Whip` |
| `Item153` | `Item_153` | — | — | 共享通用语义 | 鞭子抽响 · `Whip` |
| `Item154` | `Item_154` | — | — | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Item155` | `Item_155` | — | — | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Item156` | `Item_156` | — | — | 准确语义 | 烟花发射 · `FireworksLaunch` |
| `Item157` | `Item_157` | — | — | 共享通用语义 | 激光发射 · `LaserFire` |
| `Item158` | `Item_158` | — | — | 共享通用语义 | 激光发射 · `LaserFire` |
| `Item159` | `Item_159` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item16` | `Item_16` | — | — | 共享通用语义 | 噗噗声 · `Fart` |
| `Item160` | `Item_160` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item161` | `Item_161` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item162` | `Item_162` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item163` | `Item_163` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item164` | `Item_164` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item165` | `Item_165` | — | — | 共享通用语义 | 光之女皇发声 · `EmpressSound` |
| `Item166` | `Item_166` | — | — | 准确语义 | 八音盒录制完成 · `MusicRecorded` |
| `Item167` | `Item_167` | — | — | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Item168` | `Item_168` | — | — | 准确语义 | 弹簧高跷弹跳 · `Pogo` |
| `Item169` | `Item_169` | — | — | 共享通用语义 | 利刃挥动 · `BladeSwing` |
| `Item17` | `Item_17` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item170` | `Item_170` | — | — | 共享通用语义 | 恐惧鹦鹉螺发声 · `NautilusSound` |
| `Item171` | `Item_171` | — | — | 共享通用语义 | 恐惧鹦鹉螺发声 · `NautilusSound` |
| `Item172` | `Item_172` | — | — | 共享通用语义 | 恐惧鹦鹉螺发声 · `NautilusSound` |
| `Item173` | `Item_173` | — | — | 共享通用语义 | 蜜蜂嗡鸣 · `BeeBuzz` |
| `Item174` | `Item_174` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item175` | `Item_175` | — | — | 准确语义 | 拍击声 · `Slap` |
| `Item176` | `Item_176` | — | — | 共享通用语义 | 微光发声 · `ShimmerSound` |
| `Item177` | `Item_177` | — | — | 共享通用语义 | 黏软物受压 · `Squish` |
| `Item178` | `Item_178` | — | — | 共享通用语义 | 铁锅敲击声 · `PanImpact` |
| `Item18` | `Item_18` | — | — | 共享通用语义 | 挥动 · `Swing` |
| `Item19` | `Item_19` | — | — | 共享通用语义 | 挥动 · `Swing` |
| `Item2` | `Item_2` | — | — | 共享通用语义 | 使用物品 · `UseItem` |
| `Item20` | `Item_20` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item21` | `Item_21` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item22` | `Item_22` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item23` | `Item_23` | — | — | 共享通用语义 | 工具运转声 · `ToolMotor` |
| `Item24` | `Item_24` | — | — | 共享通用语义 | 推进声 · `Boost` |
| `Item25` | `Item_25` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item26` | `Item_26` | — | — | 共享通用语义 | 竖琴声 · `Harp` |
| `Item27` | `Item_27` | — | — | 共享通用语义 | 冰晶碎响 · `IceChink` |
| `Item28` | `Item_28` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item29` | `Item_29` | — | — | 准确语义 | 使用魔力水晶 · `ManaCrystal` |
| `Item3` | `Item_3` | — | — | 共享通用语义 | 饮用或进食 · `Drink` |
| `Item30` | `Item_30` | — | — | 准确语义 | 魔法冰块放置 · `IcePlaced` |
| `Item31` | `Item_31` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item32` | `Item_32` | — | — | 共享通用语义 | 扑翼声 · `Wingbeat` |
| `Item33` | `Item_33` | — | — | 共享通用语义 | 激光发射 · `LaserFire` |
| `Item34` | `Item_34` | — | — | 共享通用语义 | 喷射声 · `Spray` |
| `Item35` | `Item_35` | — | — | 准确语义 | 铃声 · `Bell` |
| `Item36` | `Item_36` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item37` | `Item_37` | — | — | 共享通用语义 | 金属铮鸣 · `MetalRing` |
| `Item38` | `Item_38` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item39` | `Item_39` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item4` | `Item_4` | — | — | 共享通用语义 | 清脆铃音 · `Chime` |
| `Item40` | `Item_40` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item41` | `Item_41` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item42` | `Item_42` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item43` | `Item_43` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item44` | `Item_44` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item45` | `Item_45` | — | — | 共享通用语义 | 火球命中 · `FireballImpact` |
| `Item46` | `Item_46` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item47` | `Item_47` | — | — | 共享通用语义 | 吉他和弦 · `Guitar` |
| `Item48` | `Item_48` | — | — | 共享通用语义 | 雪块受击 · `SnowImpact` |
| `Item49` | `Item_49` | — | — | 共享通用语义 | 雪块受击 · `SnowImpact` |
| `Item5` | `Item_5` | — | — | 共享通用语义 | 弓弦弹响 · `Bow` |
| `Item50` | `Item_50` | — | — | 准确语义 | 冰块受击 · `IceImpact` |
| `Item51` | `Item_51` | — | — | 准确语义 | 雪球撞击 · `SnowballImpact` |
| `Item52` | `Item_52` | — | — | 准确语义 | 矿车轨道受击 · `TrackImpact` |
| `Item53` | `Item_53` | — | — | 准确语义 | 矿车碰到轨道 · `MinecartContact` |
| `Item54` | `Item_54` | — | — | 共享通用语义 | 气泡破裂 · `BubblePop` |
| `Item55` | `Item_55` | — | — | 准确语义 | 矿车减速 · `MinecartBrake` |
| `Item56` | `Item_56` | — | — | 准确语义 | 矿车弹起 · `MinecartBounce` |
| `Item57` | `Item_57` | — | — | 共享通用语义 | 猫叫声 · `Meowmere` |
| `Item58` | `Item_58` | — | — | 共享通用语义 | 猫叫声 · `Meowmere` |
| `Item59` | `Item_59` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item6` | `Item_6` | — | — | 共享通用语义 | 传送音效 · `Teleport` |
| `Item60` | `Item_60` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item61` | `Item_61` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item62` | `Item_62` | — | — | 共享通用语义 | 爆炸 · `Explosion` |
| `Item63` | `Item_63` | — | — | 准确语义 | 吹箭发射 · `DartLaunch` |
| `Item64` | `Item_64` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item65` | `Item_65` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item66` | `Item_66` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item67` | `Item_67` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item68` | `Item_68` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item69` | `Item_69` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item7` | `Item_7` | — | — | 共享通用语义 | 武器发射声 · `WeaponDischarge` |
| `Item70` | `Item_70` | — | — | 准确语义 | 巨石撞击 · `BoulderImpact` |
| `Item71` | `Item_71` | — | — | 共享通用语义 | 利刃挥动 · `BladeSwing` |
| `Item72` | `Item_72` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item73` | `Item_73` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item74` | `Item_74` | — | — | 共享通用语义 | 魔法投射物撞击 · `MagicImpact` |
| `Item75` | `Item_75` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item76` | `Item_76` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item77` | `Item_77` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item78` | `Item_78` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item79` | `Item_79` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item8` | `Item_8` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item80` | `Item_80` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item81` | `Item_81` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item82` | `Item_82` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item83` | `Item_83` | — | — | 共享通用语义 | 召唤声 · `Summon` |
| `Item84` | `Item_84` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item85` | `Item_85` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item86` | `Item_86` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item87` | `Item_87` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item88` | `Item_88` | — | — | 共享通用语义 | 魔法武器发声 · `MagicWeapon` |
| `Item89` | `Item_89` | — | — | 准确语义 | 陨石撞击 · `MeteorImpact` |
| `Item9` | `Item_9` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item90` | `Item_90` | — | — | 准确语义 | 召唤鳞甲怪坐骑 · `ScramblerSummon` |
| `Item91` | `Item_91` | — | — | 共享通用语义 | 激光发射 · `LaserFire` |
| `Item92` | `Item_92` | — | — | 共享通用语义 | 魔法脉冲 · `MagicPulse` |
| `Item93` | `Item_93` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Item94` | `Item_94` | — | — | 共享通用语义 | 魔法投射物撞击 · `MagicImpact` |
| `Item95` | `Item_95` | — | — | 准确语义 | 外星霰弹枪发射 · `XenopopperFire` |
| `Item96` | `Item_96` | — | — | 共享通用语义 | 投射物发射 · `ProjectileLaunch` |
| `Item97` | `Item_97` | — | — | 共享通用语义 | 弓弦弹响 · `Bow` |
| `Item98` | `Item_98` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `Item99` | `Item_99` | — | — | 共享通用语义 | 枪声 · `Gunshot` |
| `JimsDrone` | `Custom/Drone` | — | — | 准确语义 | 四轴竞速无人机飞行 · `DroneFlight` |
| `Lavafall` | `Liquid_1` | — | — | 准确语义 | 熔岩流动 · `Lavafall` |
| `LiquidsHoneyLava` | `Custom/liquids_honey_lava_` | — | `0,1,2` | 共享通用语义 | 液体混合 · `LiquidMix` |
| `LiquidsHoneyWater` | `Custom/liquids_honey_water_` | — | `0,1,2` | 共享通用语义 | 液体混合 · `LiquidMix` |
| `LiquidsWaterLava` | `Custom/liquids_water_lava_` | — | `0,1,2` | 共享通用语义 | 液体混合 · `LiquidMix` |
| `LucyTheAxeTalk` | `Custom/lucyaxe_talk_` | — | `0,1,2,3,4` | 准确语义 | 露西斧说话 · `LucyTalk` |
| `Macaw` | `Zombie_` | `Terraria/Macaw` | `126,127,128` | 准确语义 | 金刚鹦鹉叫声 · `Macaw` |
| `MaxMana` | `MaxMana` | — | — | 共享通用语义 | 就绪提示音 · `MaxMana` |
| `Mech` | `Mech_0` | — | — | 共享通用语义 | 咔哒声 · `Mech` |
| `MenuClose` | `Menu_Close` | — | — | 准确语义 | 菜单关闭 · `MenuClose` |
| `MenuOpen` | `Menu_Open` | — | — | 准确语义 | 菜单打开 · `MenuOpen` |
| `MenuTick` | `Menu_Tick` | — | — | 共享通用语义 | 菜单选项提示 · `MenuTick` |
| `Meowmere` | `Item_` | `Terraria/Meowmere` | `57,58` | 共享通用语义 | 猫叫声 · `Meowmere` |
| `MoonLord` | `NPC_Killed_10` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `Mummy` | `Zombie_` | `Terraria/Mummy` | `3,4` | 准确语义 | 木乃伊低吼 · `Mummy` |
| `NPCDeath1` | `NPC_Killed_1` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath10` | `NPC_Killed_10` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath11` | `NPC_Killed_11` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath12` | `NPC_Killed_12` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath13` | `NPC_Killed_13` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath14` | `NPC_Killed_14` | — | — | 共享通用语义 | 撞击声 · `SharedImpact` |
| `NPCDeath15` | `NPC_Killed_15` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath16` | `NPC_Killed_16` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath17` | `NPC_Killed_17` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath18` | `NPC_Killed_18` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath19` | `NPC_Killed_19` | — | — | 共享通用语义 | 怪异声响 · `OddSound` |
| `NPCDeath2` | `NPC_Killed_2` | — | — | 共享通用语义 | 撞击声 · `SharedImpact` |
| `NPCDeath20` | `NPC_Killed_20` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath21` | `NPC_Killed_21` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath22` | `NPC_Killed_22` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath23` | `NPC_Killed_23` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath24` | `NPC_Killed_24` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath25` | `NPC_Killed_25` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath26` | `NPC_Killed_26` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath27` | `NPC_Killed_27` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath28` | `NPC_Killed_28` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath29` | `NPC_Killed_29` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath3` | `NPC_Killed_3` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath30` | `NPC_Killed_30` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath31` | `NPC_Killed_31` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath32` | `NPC_Killed_32` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath33` | `NPC_Killed_33` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath34` | `NPC_Killed_34` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath35` | `NPC_Killed_35` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath36` | `NPC_Killed_36` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath37` | `NPC_Killed_37` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath38` | `NPC_Killed_38` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath39` | `NPC_Killed_39` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath4` | `NPC_Killed_4` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath40` | `NPC_Killed_40` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath41` | `NPC_Killed_41` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath42` | `NPC_Killed_42` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath43` | `NPC_Killed_43` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath44` | `NPC_Killed_44` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath45` | `NPC_Killed_45` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath46` | `NPC_Killed_46` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath47` | `NPC_Killed_47` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath48` | `NPC_Killed_48` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath49` | `NPC_Killed_49` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath5` | `NPC_Killed_5` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath50` | `NPC_Killed_50` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath51` | `NPC_Killed_51` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath52` | `NPC_Killed_52` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath53` | `NPC_Killed_53` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath54` | `NPC_Killed_54` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath55` | `NPC_Killed_55` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath56` | `NPC_Killed_56` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath57` | `NPC_Killed_57` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath58` | `NPC_Killed_58` | — | — | 准确语义 | 天界柱护盾破裂 · `ShieldBreak` |
| `NPCDeath59` | `NPC_Killed_59` | — | — | 共享通用语义 | 怪异声响 · `OddSound` |
| `NPCDeath6` | `NPC_Killed_6` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath60` | `NPC_Killed_60` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath61` | `NPC_Killed_61` | — | — | 共享通用语义 | 怪异声响 · `OddSound` |
| `NPCDeath62` | `NPC_Killed_62` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath63` | `NPC_Killed_63` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath64` | `NPC_Killed_64` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath65` | `NPC_Killed_65` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath66` | `NPC_Killed_66` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath7` | `NPC_Killed_7` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath8` | `NPC_Killed_8` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCDeath9` | `NPC_Killed_9` | — | — | 共享通用语义 | 生物死亡 · `NpcDeath` |
| `NPCHit1` | `NPC_Hit_1` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit10` | `NPC_Hit_10` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit11` | `NPC_Hit_11` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit12` | `NPC_Hit_12` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit13` | `NPC_Hit_13` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit14` | `NPC_Hit_14` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit15` | `NPC_Hit_15` | — | — | 准确语义 | 训练假人受击 · `TargetHit` |
| `NPCHit16` | `NPC_Hit_16` | — | — | 准确语义 | 训练假人受击 · `TargetHit` |
| `NPCHit17` | `NPC_Hit_17` | — | — | 准确语义 | 训练假人受击 · `TargetHit` |
| `NPCHit18` | `NPC_Hit_18` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit19` | `NPC_Hit_19` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit2` | `NPC_Hit_2` | — | — | 共享通用语义 | 受击声 · `HitSound` |
| `NPCHit20` | `NPC_Hit_20` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit21` | `NPC_Hit_21` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit22` | `NPC_Hit_22` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit23` | `NPC_Hit_23` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit24` | `NPC_Hit_24` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit25` | `NPC_Hit_25` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit26` | `NPC_Hit_26` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit27` | `NPC_Hit_27` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit28` | `NPC_Hit_28` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit29` | `NPC_Hit_29` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit3` | `NPC_Hit_3` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit30` | `NPC_Hit_30` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit31` | `NPC_Hit_31` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit32` | `NPC_Hit_32` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit33` | `NPC_Hit_33` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit34` | `NPC_Hit_34` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit35` | `NPC_Hit_35` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit36` | `NPC_Hit_36` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit37` | `NPC_Hit_37` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit38` | `NPC_Hit_38` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit39` | `NPC_Hit_39` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit4` | `NPC_Hit_4` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit40` | `NPC_Hit_40` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit41` | `NPC_Hit_41` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit42` | `NPC_Hit_42` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit43` | `NPC_Hit_43` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit44` | `NPC_Hit_44` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit45` | `NPC_Hit_45` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit46` | `NPC_Hit_46` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit47` | `NPC_Hit_47` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit48` | `NPC_Hit_48` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit49` | `NPC_Hit_49` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit5` | `NPC_Hit_5` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit50` | `NPC_Hit_50` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit51` | `NPC_Hit_51` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit52` | `NPC_Hit_52` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit53` | `NPC_Hit_53` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit54` | `NPC_Hit_54` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit55` | `NPC_Hit_55` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit56` | `NPC_Hit_56` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit57` | `NPC_Hit_57` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit6` | `NPC_Hit_6` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit7` | `NPC_Hit_7` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit8` | `NPC_Hit_8` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `NPCHit9` | `NPC_Hit_9` | — | — | 共享通用语义 | 生物受伤 · `NpcHurt` |
| `Owl` | `Zombie_` | `Terraria/Owl` | `110,111,112,113,114` | 准确语义 | 猫头鹰叫声 · `Owl` |
| `Pixie` | `Pixie` | — | — | 准确语义 | 妖精嗡鸣 · `Pixie` |
| `PlayerHit` | `Player_Hit_` | — | `0,1,2` | 共享通用语义 | 玩家受伤 · `PlayerHurt` |
| `PlayerKilled` | `Player_Killed` | — | — | 准确语义 | 玩家死亡 · `PlayerKilled` |
| `QueenSlime` | `Zombie_` | `Terraria/QueenSlime` | `115,116,117` | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Research` | `Research_` | — | `1,2,3` | 准确语义 | 研究物品 · `Research` |
| `ResearchComplete` | `Research_0` | — | — | 准确语义 | 研究完成 · `ResearchComplete` |
| `Roar` | `Roar_0` | `Terraria/Roar` | — | 共享通用语义 | 吼叫 · `Roar` |
| `Run` | `Run` | — | — | 共享通用语义 | 脚步声 · `Run` |
| `SandShark` | `Zombie_7` | — | — | 共享通用语义 | 怪物叫声 · `SandShark` |
| `ScaryScream` | `Roar_2` | — | — | 共享通用语义 | 尖叫 · `Scream` |
| `Seagull` | `Zombie_` | `Terraria/Seagull` | `106,107,108` | 准确语义 | 海鸥叫声 · `Seagull` |
| `Shatter` | `Shatter` | — | — | 共享通用语义 | 碎裂 · `Shatter` |
| `Shimmer1` | `Splash_2` | — | — | 共享通用语义 | 微光溅起 · `ShimmerSplash` |
| `Shimmer2` | `Splash_3` | — | — | 准确语义 | 微光效果结束 · `ShimmerEnds` |
| `ShimmerWeak1` | `Splash_4` | `Terraria/ShimmerWeak` | — | 准确语义 | 以太块形成 · `Aetherium` |
| `ShimmerWeak2` | `Splash_5` | `Terraria/ShimmerWeak` | — | 共享通用语义 | 微光溅起 · `ShimmerSplash` |
| `Splash` | `Splash_0` | — | — | 共享通用语义 | 溅水声 · `Splash` |
| `SplashWeak` | `Splash_1` | — | — | 共享通用语义 | 液体飞溅 · `LiquidSplash` |
| `Thunder` | `Thunder_` | — | `0,1,2,3,4,5,6` | 准确语义 | 雷声 · `Thunder` |
| `Tink` | `Tink_` | — | `0,1,2` | 共享通用语义 | 硬物敲击 · `Tink` |
| `Toucan` | `Zombie_` | `Terraria/Toucan` | `129,130` | 准确语义 | 巨嘴鸟叫声 · `Toucan` |
| `Unlock` | `Unlock` | — | — | 准确语义 | 开锁 · `Unlock` |
| `Waterfall` | `Liquid_0` | — | — | 准确语义 | 瀑布声 · `Waterfall` |
| `WormDig` | `Roar_1` | — | — | 共享通用语义 | 钻地声 · `WormDig` |
| `WormDigQuiet` | `Roar_1` | — | — | 共享通用语义 | 钻地声 · `WormDig` |
| `Zombie1` | `Zombie_1` | — | — | 共享通用语义 | 低吼声 · `ZombieMoan` |
| `Zombie10` | `Zombie_10` | — | — | 准确语义 | 鸭叫 · `Duck` |
| `Zombie100` | `Zombie_100` | — | — | 共享通用语义 | 克苏鲁真眼发声 · `TrueEyeSound` |
| `Zombie101` | `Zombie_101` | — | — | 共享通用语义 | 克苏鲁真眼发声 · `TrueEyeSound` |
| `Zombie102` | `Zombie_102` | — | — | 准确语义 | 幻影球音效 · `PhantasmalSphere` |
| `Zombie103` | `Zombie_103` | — | — | 准确语义 | 幻影眼撞击 · `PhantasmalImpact` |
| `Zombie104` | `Zombie_104` | — | — | 准确语义 | 幻影死亡射线 · `PhantasmalRay` |
| `Zombie105` | `Zombie_105` | — | — | 准确语义 | 拜月教邪教徒现身 · `CultistArrival` |
| `Zombie106` | `Zombie_106` | — | — | 准确语义 | 海鸥叫声 · `Seagull` |
| `Zombie107` | `Zombie_107` | — | — | 准确语义 | 海鸥叫声 · `Seagull` |
| `Zombie108` | `Zombie_108` | — | — | 准确语义 | 海鸥叫声 · `Seagull` |
| `Zombie109` | `Zombie_109` | — | — | 准确语义 | 海豚叫声 · `Dolphin` |
| `Zombie11` | `Zombie_11` | — | — | 准确语义 | 鸭叫 · `Duck` |
| `Zombie110` | `Zombie_110` | — | — | 准确语义 | 猫头鹰叫声 · `Owl` |
| `Zombie111` | `Zombie_111` | — | — | 准确语义 | 猫头鹰叫声 · `Owl` |
| `Zombie112` | `Zombie_112` | — | — | 准确语义 | 猫头鹰叫声 · `Owl` |
| `Zombie113` | `Zombie_113` | — | — | 准确语义 | 猫头鹰叫声 · `Owl` |
| `Zombie114` | `Zombie_114` | — | — | 准确语义 | 猫头鹰叫声 · `Owl` |
| `Zombie115` | `Zombie_115` | — | — | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Zombie116` | `Zombie_116` | — | — | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Zombie117` | `Zombie_117` | — | — | 共享通用语义 | 史莱姆皇后叫声 · `QueenSlime` |
| `Zombie118` | `Zombie_118` | — | — | 准确语义 | 玄凤鹦鹉叫声 · `Cockatiel` |
| `Zombie119` | `Zombie_119` | — | — | 准确语义 | 玄凤鹦鹉叫声 · `Cockatiel` |
| `Zombie12` | `Zombie_12` | — | — | 准确语义 | 鸭叫 · `Duck` |
| `Zombie120` | `Zombie_120` | — | — | 准确语义 | 玄凤鹦鹉叫声 · `Cockatiel` |
| `Zombie121` | `Zombie_121` | — | — | 准确语义 | 小丑叫声 · `Clown` |
| `Zombie122` | `Zombie_122` | — | — | 准确语义 | 小丑叫声 · `Clown` |
| `Zombie123` | `Zombie_123` | — | — | 准确语义 | 小丑叫声 · `Clown` |
| `Zombie124` | `Zombie_124` | — | — | 准确语义 | 牙齿咔哒声 · `ChatteringTeeth` |
| `Zombie125` | `Zombie_125` | — | — | 准确语义 | 蜂王发声 · `QueenBeeSound` |
| `Zombie126` | `Zombie_126` | — | — | 准确语义 | 金刚鹦鹉叫声 · `Macaw` |
| `Zombie127` | `Zombie_127` | — | — | 准确语义 | 金刚鹦鹉叫声 · `Macaw` |
| `Zombie128` | `Zombie_128` | — | — | 准确语义 | 金刚鹦鹉叫声 · `Macaw` |
| `Zombie129` | `Zombie_129` | — | — | 准确语义 | 巨嘴鸟叫声 · `Toucan` |
| `Zombie13` | `Zombie_13` | — | — | 准确语义 | 蛙鸣 · `Frog` |
| `Zombie130` | `Zombie_130` | — | — | 准确语义 | 巨嘴鸟叫声 · `Toucan` |
| `Zombie14` | `Zombie_14` | — | — | 共享通用语义 | 鸟鸣 · `Bird` |
| `Zombie15` | `Zombie_15` | — | — | 共享通用语义 | 小动物叫声 · `Critter` |
| `Zombie16` | `Zombie_16` | — | — | 共享通用语义 | 鸟鸣 · `Bird` |
| `Zombie17` | `Zombie_17` | — | — | 共享通用语义 | 鸟鸣 · `Bird` |
| `Zombie18` | `Zombie_18` | — | — | 共享通用语义 | 鸟鸣 · `Bird` |
| `Zombie19` | `Zombie_19` | — | — | 共享通用语义 | 鸟鸣 · `Bird` |
| `Zombie2` | `Zombie_2` | — | — | 共享通用语义 | 低吼声 · `ZombieMoan` |
| `Zombie20` | `Zombie_20` | — | — | 准确语义 | 猪龙鱼公爵发声 · `FishronSound` |
| `Zombie21` | `Zombie_21` | — | — | 共享通用语义 | 血月怪物低吼 · `BloodZombie` |
| `Zombie22` | `Zombie_22` | — | — | 共享通用语义 | 血月怪物低吼 · `BloodZombie` |
| `Zombie23` | `Zombie_23` | — | — | 共享通用语义 | 血月怪物低吼 · `BloodZombie` |
| `Zombie24` | `Zombie_24` | — | — | 准确语义 | 血爬虫发声 · `BloodCrawler` |
| `Zombie25` | `Zombie_25` | — | — | 准确语义 | 血爬虫发声 · `BloodCrawler` |
| `Zombie26` | `Zombie_26` | — | — | 共享通用语义 | 恶魔发声 · `DemonSound` |
| `Zombie27` | `Zombie_27` | — | — | 共享通用语义 | 恶魔发声 · `DemonSound` |
| `Zombie28` | `Zombie_28` | — | — | 共享通用语义 | 恶魔发声 · `DemonSound` |
| `Zombie29` | `Zombie_29` | — | — | 共享通用语义 | 恶魔发声 · `DemonSound` |
| `Zombie3` | `Zombie_3` | — | — | 准确语义 | 木乃伊低吼 · `Mummy` |
| `Zombie30` | `Zombie_30` | — | — | 准确语义 | 跳跳兽发声 · `DerplingSound` |
| `Zombie31` | `Zombie_31` | — | — | 准确语义 | 跳跳兽发声 · `DerplingSound` |
| `Zombie32` | `Zombie_32` | — | — | 准确语义 | 飞蛇发声 · `FlyingSnakeSound` |
| `Zombie33` | `Zombie_33` | — | — | 准确语义 | 巨型陆龟发声 · `TortoiseSound` |
| `Zombie34` | `Zombie_34` | — | — | 共享通用语义 | 水母发声 · `JellyfishSound` |
| `Zombie35` | `Zombie_35` | — | — | 共享通用语义 | 水母发声 · `JellyfishSound` |
| `Zombie36` | `Zombie_36` | — | — | 准确语义 | 丛林蜥蜴发声 · `LihzahrdSound` |
| `Zombie37` | `Zombie_37` | — | — | 准确语义 | 丛林蜥蜴发声 · `LihzahrdSound` |
| `Zombie38` | `Zombie_38` | — | — | 准确语义 | 猪龙发声 · `Pigron` |
| `Zombie39` | `Zombie_39` | — | — | 准确语义 | 猪龙发声 · `Pigron` |
| `Zombie4` | `Zombie_4` | — | — | 准确语义 | 木乃伊低吼 · `Mummy` |
| `Zombie40` | `Zombie_40` | — | — | 准确语义 | 猪龙发声 · `Pigron` |
| `Zombie41` | `Zombie_41` | — | — | 准确语义 | 愤怒雨云怪发声 · `NimbusSound` |
| `Zombie42` | `Zombie_42` | — | — | 准确语义 | 愤怒雨云怪发声 · `NimbusSound` |
| `Zombie43` | `Zombie_43` | — | — | 准确语义 | 愤怒雨云怪发声 · `NimbusSound` |
| `Zombie44` | `Zombie_44` | — | — | 共享通用语义 | 蚁狮发声 · `AntlionSound` |
| `Zombie45` | `Zombie_45` | — | — | 共享通用语义 | 蚁狮发声 · `AntlionSound` |
| `Zombie46` | `Zombie_46` | — | — | 共享通用语义 | 蚁狮发声 · `AntlionSound` |
| `Zombie47` | `Zombie_47` | — | — | 准确语义 | 龙虾发声 · `CrawdadSound` |
| `Zombie48` | `Zombie_48` | — | — | 准确语义 | 致命球发声 · `DeadlySphereSound` |
| `Zombie49` | `Zombie_49` | — | — | 准确语义 | 致命球发声 · `DeadlySphereSound` |
| `Zombie5` | `Zombie_5` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Zombie50` | `Zombie_50` | — | — | 准确语义 | 苍蝇人博士发声 · `ManFlySound` |
| `Zombie51` | `Zombie_51` | — | — | 准确语义 | 苍蝇人博士发声 · `ManFlySound` |
| `Zombie52` | `Zombie_52` | — | — | 准确语义 | 苍蝇人博士发声 · `ManFlySound` |
| `Zombie53` | `Zombie_53` | — | — | 共享通用语义 | 幽灵发声 · `SpiritSound` |
| `Zombie54` | `Zombie_54` | — | — | 共享通用语义 | 幽灵发声 · `SpiritSound` |
| `Zombie55` | `Zombie_55` | — | — | 共享通用语义 | 食尸鬼发声 · `GhoulSound` |
| `Zombie56` | `Zombie_56` | — | — | 共享通用语义 | 食尸鬼发声 · `GhoulSound` |
| `Zombie57` | `Zombie_57` | — | — | 准确语义 | 巨型卷壳怪发声 · `ShellySound` |
| `Zombie58` | `Zombie_58` | — | — | 准确语义 | 巨型卷壳怪发声 · `ShellySound` |
| `Zombie59` | `Zombie_59` | — | — | 准确语义 | 电击怪发声 · `GigazapperSound` |
| `Zombie6` | `Zombie_6` | — | — | 准确语义 | 科学怪人低吼 · `Frankenstein` |
| `Zombie60` | `Zombie_60` | — | — | 准确语义 | 电击怪发声 · `GigazapperSound` |
| `Zombie61` | `Zombie_61` | — | — | 准确语义 | 哥布林术士发声 · `GoblinWarlockSound` |
| `Zombie62` | `Zombie_62` | — | — | 准确语义 | 哥布林术士发声 · `GoblinWarlockSound` |
| `Zombie63` | `Zombie_63` | — | — | 准确语义 | 花岗岩巨人发声 · `GraniteGolemSound` |
| `Zombie64` | `Zombie_64` | — | — | 准确语义 | 花岗岩巨人发声 · `GraniteGolemSound` |
| `Zombie65` | `Zombie_65` | — | — | 准确语义 | 花岗岩巨人发声 · `GraniteGolemSound` |
| `Zombie66` | `Zombie_66` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie67` | `Zombie_67` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie68` | `Zombie_68` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie69` | `Zombie_69` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie7` | `Zombie_7` | — | — | 共享通用语义 | 怪物叫声 · `SandShark` |
| `Zombie70` | `Zombie_70` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie71` | `Zombie_71` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie72` | `Zombie_72` | — | — | 共享通用语义 | 火星机械声 · `MartianMachine` |
| `Zombie73` | `Zombie_73` | — | — | 准确语义 | 蛾怪发声 · `MothronSound` |
| `Zombie74` | `Zombie_74` | — | — | 准确语义 | 蘑菇瓢虫发声 · `MushiSound` |
| `Zombie75` | `Zombie_75` | — | — | 准确语义 | 蘑菇瓢虫发声 · `MushiSound` |
| `Zombie76` | `Zombie_76` | — | — | 准确语义 | 蘑菇瓢虫发声 · `MushiSound` |
| `Zombie77` | `Zombie_77` | — | — | 准确语义 | 蘑菇瓢虫发声 · `MushiSound` |
| `Zombie78` | `Zombie_78` | — | — | 准确语义 | 鹦鹉叫声 · `Parrot` |
| `Zombie79` | `Zombie_79` | — | — | 准确语义 | 攀爬魔发声 · `PossessedSound` |
| `Zombie8` | `Zombie_8` | — | — | 准确语义 | 脸怪发声 · `FaceMonster` |
| `Zombie80` | `Zombie_80` | — | — | 准确语义 | 攀爬魔发声 · `PossessedSound` |
| `Zombie81` | `Zombie_81` | — | — | 共享通用语义 | 幽灵发声 · `SpiritSound` |
| `Zombie82` | `Zombie_82` | — | — | 共享通用语义 | 幽灵发声 · `SpiritSound` |
| `Zombie83` | `Zombie_83` | — | — | 共享通用语义 | 幽灵发声 · `SpiritSound` |
| `Zombie84` | `Zombie_84` | — | — | 准确语义 | 蝾螈发声 · `SalamanderSound` |
| `Zombie85` | `Zombie_85` | — | — | 准确语义 | 蝾螈发声 · `SalamanderSound` |
| `Zombie86` | `Zombie_86` | — | — | 准确语义 | 鳞甲怪发声 · `ScutlixSound` |
| `Zombie87` | `Zombie_87` | — | — | 原版未使用 | 原版音效 · `UnmappedVanilla` |
| `Zombie88` | `Zombie_88` | — | — | 准确语义 | 拜月教邪教徒发声 · `CultistSound` |
| `Zombie89` | `Zombie_89` | — | — | 准确语义 | 拜月教邪教徒发声 · `CultistSound` |
| `Zombie9` | `Zombie_9` | — | — | 准确语义 | 猪龙发声 · `Pigron` |
| `Zombie90` | `Zombie_90` | — | — | 准确语义 | 拜月教邪教徒发声 · `CultistSound` |
| `Zombie91` | `Zombie_91` | — | — | 准确语义 | 拜月教邪教徒发声 · `CultistSound` |
| `Zombie92` | `Zombie_92` | — | — | 准确语义 | 月亮领主现身 · `MoonLordArrival` |
| `Zombie93` | `Zombie_93` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `Zombie94` | `Zombie_94` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `Zombie95` | `Zombie_95` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `Zombie96` | `Zombie_96` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `Zombie97` | `Zombie_97` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `Zombie98` | `Zombie_98` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `Zombie99` | `Zombie_99` | — | — | 共享通用语义 | 月亮领主发声 · `MoonLordSound` |
| `ZombieMoan` | `Zombie_` | `Terraria/ZombieMoan` | `0,1,2` | 共享通用语义 | 低吼声 · `ZombieMoan` |

## 原版 XNB 音效资源

共 863 项，764 项能通过公开 SoundID 路径及变体回连。其余 99 项中，89 项按明确资源路径提供字幕，10 项由 Wiki 标为原版未使用。直接路径命名仅在客户端实际播放该资源时生效，原版资源的可用性仍取决于客户端版本。别名列描述使用此文件的 SoundStyle。

| 资源路径 | 关联 SoundID | 状态或直接路径字幕 |
| --- | --- | --- |
| `Camera` | `Camera` | 准确语义 |
| `Chat` | `Chat` | 准确语义 |
| `Coin_0` | `CoinPickup` | 共享通用语义 |
| `Coin_1` | `CoinPickup` | 共享通用语义 |
| `Coin_2` | `CoinPickup` | 共享通用语义 |
| `Coin_3` | `CoinPickup` | 共享通用语义 |
| `Coin_4` | `CoinPickup` | 共享通用语义 |
| `Coins` | `Coins` | 共享通用语义 |
| `Custom/abigail_attack` | `AbigailAttack` | 准确语义 |
| `Custom/abigail_cry_0` | `AbigailCry` | 准确语义 |
| `Custom/abigail_cry_1` | `AbigailCry` | 准确语义 |
| `Custom/abigail_cry_2` | `AbigailCry` | 准确语义 |
| `Custom/abigail_summon` | `AbigailSummon` | 准确语义 |
| `Custom/abigail_upgrade_0` | `AbigailUpgrade` | 准确语义 |
| `Custom/abigail_upgrade_1` | `AbigailUpgrade` | 准确语义 |
| `Custom/abigail_upgrade_2` | `AbigailUpgrade` | 准确语义 |
| `Custom/achievement_complete` | `AchievementComplete` | 准确语义 |
| `Custom/best_reforge` | — | 准确语义 · 最佳重铸提示 · `BestReforge` |
| `Custom/blizzard_inside_building_loop` | `BlizzardInsideBuildingLoop` | 共享通用语义 |
| `Custom/blizzard_strong_loop` | `BlizzardStrongLoop` | 共享通用语义 |
| `Custom/chester_close_0` | `ChesterClose` | 准确语义 |
| `Custom/chester_close_1` | `ChesterClose` | 准确语义 |
| `Custom/chester_open_0` | `ChesterOpen` | 准确语义 |
| `Custom/chester_open_1` | `ChesterOpen` | 准确语义 |
| `Custom/dd2_ballista_tower_shot_0` | `DD2_BallistaTowerShot` | 准确语义 |
| `Custom/dd2_ballista_tower_shot_1` | `DD2_BallistaTowerShot` | 准确语义 |
| `Custom/dd2_ballista_tower_shot_2` | `DD2_BallistaTowerShot` | 准确语义 |
| `Custom/dd2_betsy_death_0` | `DD2_BetsyDeath` | 共享通用语义 |
| `Custom/dd2_betsy_death_1` | `DD2_BetsyDeath` | 共享通用语义 |
| `Custom/dd2_betsy_death_2` | `DD2_BetsyDeath` | 共享通用语义 |
| `Custom/dd2_betsy_fireball_impact_0` | `DD2_BetsyFireballImpact`、`DD2_BetsysWrathImpact` | 共享通用语义 |
| `Custom/dd2_betsy_fireball_impact_1` | `DD2_BetsyFireballImpact`、`DD2_BetsysWrathImpact` | 共享通用语义 |
| `Custom/dd2_betsy_fireball_impact_2` | `DD2_BetsyFireballImpact`、`DD2_BetsysWrathImpact` | 共享通用语义 |
| `Custom/dd2_betsy_fireball_shot_0` | `DD2_BetsyFireballShot`、`DD2_BetsysWrathShot` | 共享通用语义 |
| `Custom/dd2_betsy_fireball_shot_1` | `DD2_BetsyFireballShot`、`DD2_BetsysWrathShot` | 共享通用语义 |
| `Custom/dd2_betsy_fireball_shot_2` | `DD2_BetsyFireballShot`、`DD2_BetsysWrathShot` | 共享通用语义 |
| `Custom/dd2_betsy_flame_breath` | `DD2_BetsyFlameBreath` | 准确语义 |
| `Custom/dd2_betsy_flying_circle_attack` | `DD2_BetsyFlyingCircleAttack` | 准确语义 |
| `Custom/dd2_betsy_hurt_0` | `DD2_BetsyHurt` | 共享通用语义 |
| `Custom/dd2_betsy_hurt_1` | `DD2_BetsyHurt` | 共享通用语义 |
| `Custom/dd2_betsy_hurt_2` | `DD2_BetsyHurt` | 共享通用语义 |
| `Custom/dd2_betsy_scream` | `DD2_BetsyScream` | 共享通用语义 |
| `Custom/dd2_betsy_summon_0` | `DD2_BetsySummon` | 准确语义 |
| `Custom/dd2_betsy_summon_1` | `DD2_BetsySummon` | 准确语义 |
| `Custom/dd2_betsy_summon_2` | `DD2_BetsySummon` | 准确语义 |
| `Custom/dd2_betsy_wind_attack_0` | `DD2_BetsyWindAttack` | 准确语义 |
| `Custom/dd2_betsy_wind_attack_1` | `DD2_BetsyWindAttack` | 准确语义 |
| `Custom/dd2_betsy_wind_attack_2` | `DD2_BetsyWindAttack` | 准确语义 |
| `Custom/dd2_book_staff_cast_0` | `DD2_BookStaffCast` | 准确语义 |
| `Custom/dd2_book_staff_cast_1` | `DD2_BookStaffCast` | 准确语义 |
| `Custom/dd2_book_staff_cast_2` | `DD2_BookStaffCast` | 准确语义 |
| `Custom/dd2_book_staff_twister_loop` | `DD2_BookStaffTwisterLoop` | 准确语义 |
| `Custom/dd2_crystal_cart_impact_0` | `DD2_CrystalCartImpact` | 准确语义 |
| `Custom/dd2_crystal_cart_impact_1` | `DD2_CrystalCartImpact` | 准确语义 |
| `Custom/dd2_crystal_cart_impact_2` | `DD2_CrystalCartImpact` | 准确语义 |
| `Custom/dd2_dark_mage_attack_0` | `DD2_DarkMageAttack` | 准确语义 |
| `Custom/dd2_dark_mage_attack_1` | `DD2_DarkMageAttack` | 准确语义 |
| `Custom/dd2_dark_mage_attack_2` | `DD2_DarkMageAttack` | 准确语义 |
| `Custom/dd2_dark_mage_cast_heal_0` | `DD2_DarkMageCastHeal` | 准确语义 |
| `Custom/dd2_dark_mage_cast_heal_1` | `DD2_DarkMageCastHeal` | 准确语义 |
| `Custom/dd2_dark_mage_cast_heal_2` | `DD2_DarkMageCastHeal` | 准确语义 |
| `Custom/dd2_dark_mage_death_0` | `DD2_DarkMageDeath` | 共享通用语义 |
| `Custom/dd2_dark_mage_death_1` | `DD2_DarkMageDeath` | 共享通用语义 |
| `Custom/dd2_dark_mage_death_2` | `DD2_DarkMageDeath` | 共享通用语义 |
| `Custom/dd2_dark_mage_heal_impact_0` | `DD2_DarkMageHealImpact` | 准确语义 |
| `Custom/dd2_dark_mage_heal_impact_1` | `DD2_DarkMageHealImpact` | 准确语义 |
| `Custom/dd2_dark_mage_heal_impact_2` | `DD2_DarkMageHealImpact` | 准确语义 |
| `Custom/dd2_dark_mage_hurt_0` | `DD2_DarkMageHurt` | 共享通用语义 |
| `Custom/dd2_dark_mage_hurt_1` | `DD2_DarkMageHurt` | 共享通用语义 |
| `Custom/dd2_dark_mage_hurt_2` | `DD2_DarkMageHurt` | 共享通用语义 |
| `Custom/dd2_dark_mage_summon_skeleton_0` | `DD2_DarkMageSummonSkeleton` | 共享通用语义 |
| `Custom/dd2_dark_mage_summon_skeleton_1` | `DD2_DarkMageSummonSkeleton` | 共享通用语义 |
| `Custom/dd2_dark_mage_summon_skeleton_2` | `DD2_DarkMageSummonSkeleton` | 共享通用语义 |
| `Custom/dd2_defeat_scene` | `DD2_DefeatScene` | 准确语义 |
| `Custom/dd2_defense_tower_spawn` | `DD2_DefenseTowerSpawn` | 共享通用语义 |
| `Custom/dd2_drakin_breath_in_0` | `DD2_DrakinBreathIn` | 准确语义 |
| `Custom/dd2_drakin_breath_in_1` | `DD2_DrakinBreathIn` | 准确语义 |
| `Custom/dd2_drakin_breath_in_2` | `DD2_DrakinBreathIn` | 准确语义 |
| `Custom/dd2_drakin_death_0` | `DD2_DrakinDeath` | 共享通用语义 |
| `Custom/dd2_drakin_death_1` | `DD2_DrakinDeath` | 共享通用语义 |
| `Custom/dd2_drakin_death_2` | `DD2_DrakinDeath` | 共享通用语义 |
| `Custom/dd2_drakin_hurt_0` | `DD2_DrakinHurt` | 共享通用语义 |
| `Custom/dd2_drakin_hurt_1` | `DD2_DrakinHurt` | 共享通用语义 |
| `Custom/dd2_drakin_hurt_2` | `DD2_DrakinHurt` | 共享通用语义 |
| `Custom/dd2_drakin_shot_0` | `DD2_DrakinShot` | 准确语义 |
| `Custom/dd2_drakin_shot_1` | `DD2_DrakinShot` | 准确语义 |
| `Custom/dd2_drakin_shot_2` | `DD2_DrakinShot` | 准确语义 |
| `Custom/dd2_etherian_portal_dryad_touch` | `DD2_EtherianPortalDryadTouch` | 原版未使用 |
| `Custom/dd2_etherian_portal_idle_loop` | `DD2_EtherianPortalIdleLoop` | 准确语义 |
| `Custom/dd2_etherian_portal_open` | `DD2_EtherianPortalOpen` | 准确语义 |
| `Custom/dd2_etherian_portal_spawn_enemy_0` | `DD2_EtherianPortalSpawnEnemy` | 准确语义 |
| `Custom/dd2_etherian_portal_spawn_enemy_1` | `DD2_EtherianPortalSpawnEnemy` | 准确语义 |
| `Custom/dd2_etherian_portal_spawn_enemy_2` | `DD2_EtherianPortalSpawnEnemy` | 准确语义 |
| `Custom/dd2_explosive_trap_explode_0` | `DD2_ExplosiveTrapExplode` | 共享通用语义 |
| `Custom/dd2_explosive_trap_explode_1` | `DD2_ExplosiveTrapExplode` | 共享通用语义 |
| `Custom/dd2_explosive_trap_explode_2` | `DD2_ExplosiveTrapExplode` | 共享通用语义 |
| `Custom/dd2_flameburst_tower_shot_0` | `DD2_FlameburstTowerShot` | 准确语义 |
| `Custom/dd2_flameburst_tower_shot_1` | `DD2_FlameburstTowerShot` | 准确语义 |
| `Custom/dd2_flameburst_tower_shot_2` | `DD2_FlameburstTowerShot` | 准确语义 |
| `Custom/dd2_ghastly_glaive_impact_ghost_0` | `DD2_GhastlyGlaiveImpactGhost` | 准确语义 |
| `Custom/dd2_ghastly_glaive_impact_ghost_1` | `DD2_GhastlyGlaiveImpactGhost` | 准确语义 |
| `Custom/dd2_ghastly_glaive_impact_ghost_2` | `DD2_GhastlyGlaiveImpactGhost` | 准确语义 |
| `Custom/dd2_ghastly_glaive_pierce_0` | `DD2_GhastlyGlaivePierce` | 准确语义 |
| `Custom/dd2_ghastly_glaive_pierce_1` | `DD2_GhastlyGlaivePierce` | 准确语义 |
| `Custom/dd2_ghastly_glaive_pierce_2` | `DD2_GhastlyGlaivePierce` | 准确语义 |
| `Custom/dd2_goblin_bomber_death_0` | `DD2_GoblinBomberDeath` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_death_1` | `DD2_GoblinBomberDeath` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_death_2` | `DD2_GoblinBomberDeath` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_hurt_0` | `DD2_GoblinBomberHurt` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_hurt_1` | `DD2_GoblinBomberHurt` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_hurt_2` | `DD2_GoblinBomberHurt` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_scream_0` | `DD2_GoblinBomberScream` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_scream_1` | `DD2_GoblinBomberScream` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_scream_2` | `DD2_GoblinBomberScream` | 共享通用语义 |
| `Custom/dd2_goblin_bomber_throw_0` | `DD2_GoblinBomberThrow` | 准确语义 |
| `Custom/dd2_goblin_bomber_throw_1` | `DD2_GoblinBomberThrow` | 准确语义 |
| `Custom/dd2_goblin_bomber_throw_2` | `DD2_GoblinBomberThrow` | 准确语义 |
| `Custom/dd2_goblin_death_0` | `DD2_GoblinDeath` | 共享通用语义 |
| `Custom/dd2_goblin_death_1` | `DD2_GoblinDeath` | 共享通用语义 |
| `Custom/dd2_goblin_death_2` | `DD2_GoblinDeath` | 共享通用语义 |
| `Custom/dd2_goblin_hurt_0` | `DD2_GoblinHurt` | 共享通用语义 |
| `Custom/dd2_goblin_hurt_1` | `DD2_GoblinHurt` | 共享通用语义 |
| `Custom/dd2_goblin_hurt_2` | `DD2_GoblinHurt` | 共享通用语义 |
| `Custom/dd2_goblin_hurt_3` | `DD2_GoblinHurt` | 共享通用语义 |
| `Custom/dd2_goblin_hurt_4` | `DD2_GoblinHurt` | 共享通用语义 |
| `Custom/dd2_goblin_hurt_5` | `DD2_GoblinHurt` | 共享通用语义 |
| `Custom/dd2_goblin_scream_0` | `DD2_GoblinScream` | 共享通用语义 |
| `Custom/dd2_goblin_scream_1` | `DD2_GoblinScream` | 共享通用语义 |
| `Custom/dd2_goblin_scream_2` | `DD2_GoblinScream` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_attack_0` | `DD2_JavelinThrowersAttack` | 准确语义 |
| `Custom/dd2_javelin_throwers_attack_1` | `DD2_JavelinThrowersAttack` | 准确语义 |
| `Custom/dd2_javelin_throwers_attack_2` | `DD2_JavelinThrowersAttack` | 准确语义 |
| `Custom/dd2_javelin_throwers_death_0` | `DD2_JavelinThrowersDeath` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_death_1` | `DD2_JavelinThrowersDeath` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_death_2` | `DD2_JavelinThrowersDeath` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_hurt_0` | `DD2_JavelinThrowersHurt` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_hurt_1` | `DD2_JavelinThrowersHurt` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_hurt_2` | `DD2_JavelinThrowersHurt` | 共享通用语义 |
| `Custom/dd2_javelin_throwers_taunt_0` | `DD2_JavelinThrowersTaunt` | 准确语义 |
| `Custom/dd2_javelin_throwers_taunt_1` | `DD2_JavelinThrowersTaunt` | 准确语义 |
| `Custom/dd2_javelin_throwers_taunt_2` | `DD2_JavelinThrowersTaunt` | 准确语义 |
| `Custom/dd2_kobold_death_0` | `DD2_KoboldDeath` | 共享通用语义 |
| `Custom/dd2_kobold_death_1` | `DD2_KoboldDeath` | 共享通用语义 |
| `Custom/dd2_kobold_death_2` | `DD2_KoboldDeath` | 共享通用语义 |
| `Custom/dd2_kobold_explosion_0` | `DD2_KoboldExplosion` | 共享通用语义 |
| `Custom/dd2_kobold_explosion_1` | `DD2_KoboldExplosion` | 共享通用语义 |
| `Custom/dd2_kobold_explosion_2` | `DD2_KoboldExplosion` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_charge_scream_0` | `DD2_KoboldFlyerChargeScream` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_charge_scream_1` | `DD2_KoboldFlyerChargeScream` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_charge_scream_2` | `DD2_KoboldFlyerChargeScream` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_death_0` | `DD2_KoboldFlyerDeath` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_death_1` | `DD2_KoboldFlyerDeath` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_death_2` | `DD2_KoboldFlyerDeath` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_hurt_0` | `DD2_KoboldFlyerHurt` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_hurt_1` | `DD2_KoboldFlyerHurt` | 共享通用语义 |
| `Custom/dd2_kobold_flyer_hurt_2` | `DD2_KoboldFlyerHurt` | 共享通用语义 |
| `Custom/dd2_kobold_hurt_0` | `DD2_KoboldHurt` | 共享通用语义 |
| `Custom/dd2_kobold_hurt_1` | `DD2_KoboldHurt` | 共享通用语义 |
| `Custom/dd2_kobold_hurt_2` | `DD2_KoboldHurt` | 共享通用语义 |
| `Custom/dd2_kobold_ignite_loop` | `DD2_KoboldIgniteLoop` | 准确语义 |
| `Custom/dd2_kobold_ignite` | `DD2_KoboldIgnite` | 准确语义 |
| `Custom/dd2_kobold_scream_charge_loop` | `DD2_KoboldScreamChargeLoop` | 共享通用语义 |
| `Custom/dd2_lightning_aura_zap_0` | `DD2_LightningAuraZap` | 共享通用语义 |
| `Custom/dd2_lightning_aura_zap_1` | `DD2_LightningAuraZap` | 共享通用语义 |
| `Custom/dd2_lightning_aura_zap_2` | `DD2_LightningAuraZap` | 共享通用语义 |
| `Custom/dd2_lightning_aura_zap_3` | `DD2_LightningAuraZap` | 共享通用语义 |
| `Custom/dd2_lightning_bug_death_0` | `DD2_LightningBugDeath` | 共享通用语义 |
| `Custom/dd2_lightning_bug_death_1` | `DD2_LightningBugDeath` | 共享通用语义 |
| `Custom/dd2_lightning_bug_death_2` | `DD2_LightningBugDeath` | 共享通用语义 |
| `Custom/dd2_lightning_bug_hurt_0` | `DD2_LightningBugHurt` | 共享通用语义 |
| `Custom/dd2_lightning_bug_hurt_1` | `DD2_LightningBugHurt` | 共享通用语义 |
| `Custom/dd2_lightning_bug_hurt_2` | `DD2_LightningBugHurt` | 共享通用语义 |
| `Custom/dd2_lightning_bug_zap_0` | `DD2_LightningBugZap` | 共享通用语义 |
| `Custom/dd2_lightning_bug_zap_1` | `DD2_LightningBugZap` | 共享通用语义 |
| `Custom/dd2_lightning_bug_zap_2` | `DD2_LightningBugZap` | 共享通用语义 |
| `Custom/dd2_monk_staff_ground_impact_0` | `DD2_MonkStaffGroundImpact` | 准确语义 |
| `Custom/dd2_monk_staff_ground_impact_1` | `DD2_MonkStaffGroundImpact` | 准确语义 |
| `Custom/dd2_monk_staff_ground_impact_2` | `DD2_MonkStaffGroundImpact` | 准确语义 |
| `Custom/dd2_monk_staff_ground_miss_0` | `DD2_MonkStaffGroundMiss` | 准确语义 |
| `Custom/dd2_monk_staff_ground_miss_1` | `DD2_MonkStaffGroundMiss` | 准确语义 |
| `Custom/dd2_monk_staff_ground_miss_2` | `DD2_MonkStaffGroundMiss` | 准确语义 |
| `Custom/dd2_monk_staff_swing_0` | `DD2_MonkStaffSwing` | 共享通用语义 |
| `Custom/dd2_monk_staff_swing_1` | `DD2_MonkStaffSwing` | 共享通用语义 |
| `Custom/dd2_monk_staff_swing_2` | `DD2_MonkStaffSwing` | 共享通用语义 |
| `Custom/dd2_monk_staff_swing_3` | `DD2_MonkStaffSwing` | 共享通用语义 |
| `Custom/dd2_ogre_attack_0` | `DD2_OgreAttack` | 准确语义 |
| `Custom/dd2_ogre_attack_1` | `DD2_OgreAttack` | 准确语义 |
| `Custom/dd2_ogre_attack_2` | `DD2_OgreAttack` | 准确语义 |
| `Custom/dd2_ogre_death_0` | `DD2_OgreDeath` | 共享通用语义 |
| `Custom/dd2_ogre_death_1` | `DD2_OgreDeath` | 共享通用语义 |
| `Custom/dd2_ogre_death_2` | `DD2_OgreDeath` | 共享通用语义 |
| `Custom/dd2_ogre_ground_pound` | `DD2_OgreGroundPound` | 准确语义 |
| `Custom/dd2_ogre_hurt_0` | `DD2_OgreHurt` | 共享通用语义 |
| `Custom/dd2_ogre_hurt_1` | `DD2_OgreHurt` | 共享通用语义 |
| `Custom/dd2_ogre_hurt_2` | `DD2_OgreHurt` | 共享通用语义 |
| `Custom/dd2_ogre_roar_0` | `DD2_OgreRoar` | 共享通用语义 |
| `Custom/dd2_ogre_roar_1` | `DD2_OgreRoar` | 共享通用语义 |
| `Custom/dd2_ogre_roar_2` | `DD2_OgreRoar` | 共享通用语义 |
| `Custom/dd2_ogre_spit` | `DD2_OgreSpit` | 准确语义 |
| `Custom/dd2_phantom_phoenix_shot_0` | `DD2_PhantomPhoenixShot` | 准确语义 |
| `Custom/dd2_phantom_phoenix_shot_1` | `DD2_PhantomPhoenixShot` | 准确语义 |
| `Custom/dd2_phantom_phoenix_shot_2` | `DD2_PhantomPhoenixShot` | 准确语义 |
| `Custom/dd2_skeleton_death_0` | `DD2_SkeletonDeath` | 共享通用语义 |
| `Custom/dd2_skeleton_death_1` | `DD2_SkeletonDeath` | 共享通用语义 |
| `Custom/dd2_skeleton_death_2` | `DD2_SkeletonDeath` | 共享通用语义 |
| `Custom/dd2_skeleton_hurt_0` | `DD2_SkeletonHurt` | 共享通用语义 |
| `Custom/dd2_skeleton_hurt_1` | `DD2_SkeletonHurt` | 共享通用语义 |
| `Custom/dd2_skeleton_hurt_2` | `DD2_SkeletonHurt` | 共享通用语义 |
| `Custom/dd2_skeleton_summoned` | `DD2_SkeletonSummoned` | 共享通用语义 |
| `Custom/dd2_sky_dragons_fury_circle_0` | `DD2_SkyDragonsFuryCircle` | 准确语义 |
| `Custom/dd2_sky_dragons_fury_circle_1` | `DD2_SkyDragonsFuryCircle` | 准确语义 |
| `Custom/dd2_sky_dragons_fury_circle_2` | `DD2_SkyDragonsFuryCircle` | 准确语义 |
| `Custom/dd2_sky_dragons_fury_shot_0` | `DD2_SkyDragonsFuryShot` | 准确语义 |
| `Custom/dd2_sky_dragons_fury_shot_1` | `DD2_SkyDragonsFuryShot` | 准确语义 |
| `Custom/dd2_sky_dragons_fury_shot_2` | `DD2_SkyDragonsFuryShot` | 准确语义 |
| `Custom/dd2_sky_dragons_fury_swing_0` | `DD2_SkyDragonsFurySwing` | 共享通用语义 |
| `Custom/dd2_sky_dragons_fury_swing_1` | `DD2_SkyDragonsFurySwing` | 共享通用语义 |
| `Custom/dd2_sky_dragons_fury_swing_2` | `DD2_SkyDragonsFurySwing` | 共享通用语义 |
| `Custom/dd2_sky_dragons_fury_swing_3` | `DD2_SkyDragonsFurySwing` | 共享通用语义 |
| `Custom/dd2_sonic_boom_blade_slash_0` | `DD2_SonicBoomBladeSlash` | 准确语义 |
| `Custom/dd2_sonic_boom_blade_slash_1` | `DD2_SonicBoomBladeSlash` | 准确语义 |
| `Custom/dd2_sonic_boom_blade_slash_2` | `DD2_SonicBoomBladeSlash` | 准确语义 |
| `Custom/dd2_win_scene` | `DD2_WinScene` | 准确语义 |
| `Custom/dd2_wither_beast_aura_pulse_0` | `DD2_WitherBeastAuraPulse` | 准确语义 |
| `Custom/dd2_wither_beast_aura_pulse_1` | `DD2_WitherBeastAuraPulse` | 准确语义 |
| `Custom/dd2_wither_beast_crystal_impact_0` | `DD2_WitherBeastCrystalImpact` | 准确语义 |
| `Custom/dd2_wither_beast_crystal_impact_1` | `DD2_WitherBeastCrystalImpact` | 准确语义 |
| `Custom/dd2_wither_beast_crystal_impact_2` | `DD2_WitherBeastCrystalImpact` | 准确语义 |
| `Custom/dd2_wither_beast_death_0` | `DD2_WitherBeastDeath` | 共享通用语义 |
| `Custom/dd2_wither_beast_death_1` | `DD2_WitherBeastDeath` | 共享通用语义 |
| `Custom/dd2_wither_beast_death_2` | `DD2_WitherBeastDeath` | 共享通用语义 |
| `Custom/dd2_wither_beast_hurt_0` | `DD2_WitherBeastHurt` | 共享通用语义 |
| `Custom/dd2_wither_beast_hurt_1` | `DD2_WitherBeastHurt` | 共享通用语义 |
| `Custom/dd2_wither_beast_hurt_2` | `DD2_WitherBeastHurt` | 共享通用语义 |
| `Custom/dd2_wyvern_death_0` | `DD2_WyvernDeath` | 共享通用语义 |
| `Custom/dd2_wyvern_death_1` | `DD2_WyvernDeath` | 共享通用语义 |
| `Custom/dd2_wyvern_death_2` | `DD2_WyvernDeath` | 共享通用语义 |
| `Custom/dd2_wyvern_dive_down_0` | `DD2_WyvernDiveDown` | 准确语义 |
| `Custom/dd2_wyvern_dive_down_1` | `DD2_WyvernDiveDown` | 准确语义 |
| `Custom/dd2_wyvern_dive_down_2` | `DD2_WyvernDiveDown` | 准确语义 |
| `Custom/dd2_wyvern_hurt_0` | `DD2_WyvernHurt` | 共享通用语义 |
| `Custom/dd2_wyvern_hurt_1` | `DD2_WyvernHurt` | 共享通用语义 |
| `Custom/dd2_wyvern_hurt_2` | `DD2_WyvernHurt` | 共享通用语义 |
| `Custom/dd2_wyvern_scream_0` | `DD2_WyvernScream` | 共享通用语义 |
| `Custom/dd2_wyvern_scream_1` | `DD2_WyvernScream` | 共享通用语义 |
| `Custom/dd2_wyvern_scream_2` | `DD2_WyvernScream` | 共享通用语义 |
| `Custom/deadcells_barrel_launcher_explode` | — | 共享通用语义 · 爆炸 · `Explosion` |
| `Custom/deadcells_barrel_launcher_fire` | — | 准确语义 · 桶发射器发射 · `BarrelFire` |
| `Custom/deadcells_flint_charge` | — | 准确语义 · 燧石蓄力 · `FlintCharge` |
| `Custom/deadcells_flint_release` | — | 准确语义 · 燧石释放攻击 · `FlintRelease` |
| `Custom/deadcells_flint_wave` | — | 准确语义 · 燧石冲击波 · `FlintWave` |
| `Custom/deadcells_mushroom_explode` | — | 共享通用语义 · 爆炸 · `Explosion` |
| `Custom/deadcells_mushroom_jump` | — | 原版未使用 |
| `Custom/deadcells_mushroom_land` | — | 原版未使用 |
| `Custom/deadcells_mushroom_summon` | — | 准确语义 · 召唤蘑菇伙伴 · `MushroomSummon` |
| `Custom/deerclops_death` | `DeerclopsDeath` | 准确语义 |
| `Custom/deerclops_hit_0` | `DeerclopsHit` | 准确语义 |
| `Custom/deerclops_hit_1` | `DeerclopsHit` | 准确语义 |
| `Custom/deerclops_hit_2` | `DeerclopsHit` | 准确语义 |
| `Custom/deerclops_ice_attack_0` | `DeerclopsIceAttack` | 准确语义 |
| `Custom/deerclops_ice_attack_1` | `DeerclopsIceAttack` | 准确语义 |
| `Custom/deerclops_ice_attack_2` | `DeerclopsIceAttack` | 准确语义 |
| `Custom/deerclops_rubble_attack` | `DeerclopsRubbleAttack` | 准确语义 |
| `Custom/deerclops_scream_0` | `DeerclopsScream` | 准确语义 |
| `Custom/deerclops_scream_1` | `DeerclopsScream` | 准确语义 |
| `Custom/deerclops_scream_2` | `DeerclopsScream` | 准确语义 |
| `Custom/deerclops_step` | `DeerclopsStep` | 准确语义 |
| `Custom/Drone` | `JimsDrone` | 准确语义 |
| `Custom/dst_female_hit_0` | `DSTFemaleHurt` | 共享通用语义 |
| `Custom/dst_female_hit_1` | `DSTFemaleHurt` | 共享通用语义 |
| `Custom/dst_female_hit_2` | `DSTFemaleHurt` | 共享通用语义 |
| `Custom/dst_male_hit_0` | `DSTMaleHurt` | 共享通用语义 |
| `Custom/dst_male_hit_1` | `DSTMaleHurt` | 共享通用语义 |
| `Custom/dst_male_hit_2` | `DSTMaleHurt` | 共享通用语义 |
| `Custom/eow_dig` | — | 准确语义 · 世界吞噬怪钻地 · `EaterDig` |
| `Custom/fish_splash` | — | 准确语义 · 钓鱼溅水声 · `FishingSplash` |
| `Custom/fuse` | — | 共享通用语义 · 引线燃烧 · `FuseBurn` |
| `Custom/glommer_bounce_0` | `GlommerBounce` | 准确语义 |
| `Custom/glommer_bounce_1` | `GlommerBounce` | 准确语义 |
| `Custom/hungry` | — | 准确语义 · 饥饿提示 · `Hunger` |
| `Custom/instant_thunder_0` | — | 共享通用语义 · 雷声 · `Thunder` |
| `Custom/instant_thunder_1` | — | 共享通用语义 · 雷声 · `Thunder` |
| `Custom/instant_thunder_2` | — | 共享通用语义 · 雷声 · `Thunder` |
| `Custom/liquids_honey_lava_0` | `LiquidsHoneyLava` | 共享通用语义 |
| `Custom/liquids_honey_lava_1` | `LiquidsHoneyLava` | 共享通用语义 |
| `Custom/liquids_honey_lava_2` | `LiquidsHoneyLava` | 共享通用语义 |
| `Custom/liquids_honey_water_0` | `LiquidsHoneyWater` | 共享通用语义 |
| `Custom/liquids_honey_water_1` | `LiquidsHoneyWater` | 共享通用语义 |
| `Custom/liquids_honey_water_2` | `LiquidsHoneyWater` | 共享通用语义 |
| `Custom/liquids_water_lava_0` | `LiquidsWaterLava` | 共享通用语义 |
| `Custom/liquids_water_lava_1` | `LiquidsWaterLava` | 共享通用语义 |
| `Custom/liquids_water_lava_2` | `LiquidsWaterLava` | 共享通用语义 |
| `Custom/lucyaxe_talk_0` | `LucyTheAxeTalk` | 准确语义 |
| `Custom/lucyaxe_talk_1` | `LucyTheAxeTalk` | 准确语义 |
| `Custom/lucyaxe_talk_2` | `LucyTheAxeTalk` | 准确语义 |
| `Custom/lucyaxe_talk_3` | `LucyTheAxeTalk` | 准确语义 |
| `Custom/lucyaxe_talk_4` | `LucyTheAxeTalk` | 准确语义 |
| `Custom/menu_accept` | — | 准确语义 · 菜单确认 · `MenuAccept` |
| `Custom/meteor_shower_0` | — | 准确语义 · 流星雨音效 · `MeteorShower` |
| `Custom/meteor_shower_1` | — | 准确语义 · 流星雨音效 · `MeteorShower` |
| `Custom/meteor_shower_2` | — | 准确语义 · 流星雨音效 · `MeteorShower` |
| `Custom/meteor_shower_3` | — | 准确语义 · 流星雨音效 · `MeteorShower` |
| `Custom/meteor_shower_4` | — | 准确语义 · 流星雨音效 · `MeteorShower` |
| `Custom/meteor_shower_5` | — | 准确语义 · 流星雨音效 · `MeteorShower` |
| `Custom/pal_cattiva_joy` | — | 准确语义 · 捣蛋猫发声 · `CattivaSound` |
| `Custom/pal_cattiva_pain` | — | 准确语义 · 捣蛋猫不安叫声 · `CattivaDistress` |
| `Custom/pal_cattiva` | — | 准确语义 · 捣蛋猫发声 · `CattivaSound` |
| `Custom/pal_chillet_attack` | — | 共享通用语义 · 疾旋鼬攻击 · `ChilletAttack` |
| `Custom/pal_chillet_joy` | — | 共享通用语义 · 疾旋鼬发声 · `ChilletSound` |
| `Custom/pal_chillet` | — | 共享通用语义 · 疾旋鼬发声 · `ChilletSound` |
| `Custom/pal_digtoise_joy` | — | 准确语义 · 碎岩龟发声 · `DigtoiseSound` |
| `Custom/pal_digtoise` | — | 准确语义 · 碎岩龟发声 · `DigtoiseSound` |
| `Custom/pal_foxparks_attack` | — | 准确语义 · 火绒狐攻击 · `FoxparksAttack` |
| `Custom/pal_foxparks_joy` | — | 准确语义 · 火绒狐发声 · `FoxparksSound` |
| `Custom/pal_foxparks_pain` | — | 准确语义 · 火绒狐不安叫声 · `FoxparksDistress` |
| `Custom/pal_foxparks` | — | 准确语义 · 火绒狐发声 · `FoxparksSound` |
| `Custom/pal_summon` | — | 共享通用语义 · 召唤帕鲁 · `PalSummon` |
| `Custom/player_death_retro` | — | 准确语义 · 玩家死亡 · `PlayerKilled` |
| `Custom/player_hit_balloon` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_cat` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_chicken_rare` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_chicken` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_crow` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_default` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_dog` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_fairy` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_frog` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_goat` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_goblin` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_retro` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_turkey` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_undead` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/player_hit_vampire` | — | 共享通用语义 · 玩家受伤 · `PlayerHurt` |
| `Custom/rainbow_boulder` | — | 共享通用语义 · 彩虹音效 · `RainbowChime` |
| `Custom/rccar` | — | 准确语义 · 遥控车行驶 · `RemoteCar` |
| `Custom/sizzle` | — | 准确语义 · 阳光灼烧 · `SunlightBurn` |
| `Custom/sonar_potion` | — | 准确语义 · 声呐探测声 · `Sonar` |
| `Custom/statuemimic_jump` | — | 准确语义 · 雕像怪跳起 · `StatueJump` |
| `Custom/statuemimic_laugh_0` | — | 准确语义 · 雕像怪发笑 · `StatueLaugh` |
| `Custom/statuemimic_laugh_1` | — | 准确语义 · 雕像怪发笑 · `StatueLaugh` |
| `Custom/statuemimic_laugh_2` | — | 准确语义 · 雕像怪发笑 · `StatueLaugh` |
| `Custom/statuemimic_scare` | — | 准确语义 · 雕像怪惊吓声 · `StatueScare` |
| `Custom/trash_item_0` | — | 准确语义 · 物品丢入垃圾桶 · `TrashItem` |
| `Custom/trash_item_1` | — | 准确语义 · 物品丢入垃圾桶 · `TrashItem` |
| `Dig_0` | `Dig` | 共享通用语义 |
| `Dig_1` | `Dig` | 共享通用语义 |
| `Dig_2` | `Dig` | 共享通用语义 |
| `Door_Closed` | `DoorClosed` | 准确语义 |
| `Door_Opened` | `DoorOpen` | 共享通用语义 |
| `Double_Jump` | `DoubleJump` | 准确语义 |
| `Drip_0` | `Drip` | 共享通用语义 |
| `Drip_1` | `Drip` | 共享通用语义 |
| `Drip_2` | `DripSplash` | 共享通用语义 |
| `Drown` | `Drown` | 准确语义 |
| `Female_Hit_0` | `FemaleHit` | 共享通用语义 |
| `Female_Hit_1` | `FemaleHit` | 共享通用语义 |
| `Female_Hit_2` | `FemaleHit` | 共享通用语义 |
| `Grab` | `Grab` | 共享通用语义 |
| `Grass` | `Grass` | 共享通用语义 |
| `Item_1` | `Item1` | 共享通用语义 |
| `Item_10` | `Item10` | 共享通用语义 |
| `Item_100` | `Item100` | 共享通用语义 |
| `Item_101` | `Item101` | 共享通用语义 |
| `Item_102` | `Item102` | 共享通用语义 |
| `Item_103` | `Item103` | 共享通用语义 |
| `Item_104` | `Item104` | 原版未使用 |
| `Item_105` | `Item105` | 共享通用语义 |
| `Item_106` | `Item106` | 共享通用语义 |
| `Item_107` | `Item107` | 共享通用语义 |
| `Item_108` | `Item108` | 准确语义 |
| `Item_109` | `Item109` | 共享通用语义 |
| `Item_11` | `Item11` | 共享通用语义 |
| `Item_110` | `Item110` | 共享通用语义 |
| `Item_111` | `Item111` | 共享通用语义 |
| `Item_112` | `Item112` | 原版未使用 |
| `Item_113` | `Item113` | 共享通用语义 |
| `Item_114` | `Item114` | 共享通用语义 |
| `Item_115` | `Item115` | 共享通用语义 |
| `Item_116` | `Item116` | 共享通用语义 |
| `Item_117` | `Item117` | 共享通用语义 |
| `Item_118` | `Item118` | 共享通用语义 |
| `Item_119` | `Item119` | 准确语义 |
| `Item_12` | `Item12` | 共享通用语义 |
| `Item_120` | `Item120` | 准确语义 |
| `Item_121` | `Item121` | 共享通用语义 |
| `Item_122` | `Item122` | 原版未使用 |
| `Item_123` | `Item123` | 共享通用语义 |
| `Item_124` | `Item124` | 共享通用语义 |
| `Item_125` | `Item125` | 共享通用语义 |
| `Item_126` | `Item126` | 准确语义 |
| `Item_127` | `Item127` | 准确语义 |
| `Item_128` | `Item128` | 准确语义 |
| `Item_129` | `Item129` | 准确语义 |
| `Item_13` | `Item13` | 共享通用语义 |
| `Item_130` | `Item130` | 准确语义 |
| `Item_131` | `Item131` | 原版未使用 |
| `Item_132` | `Item132` | 共享通用语义 |
| `Item_133` | `GuitarC`、`Item133` | 共享通用语义 |
| `Item_134` | `GuitarD`、`Item134` | 共享通用语义 |
| `Item_135` | `GuitarEm`、`Item135` | 共享通用语义 |
| `Item_136` | `GuitarG`、`Item136` | 共享通用语义 |
| `Item_137` | `GuitarBm`、`Item137` | 共享通用语义 |
| `Item_138` | `GuitarAm`、`Item138` | 共享通用语义 |
| `Item_139` | `DrumHiHat`、`Item139` | 共享通用语义 |
| `Item_14` | `Item14` | 共享通用语义 |
| `Item_140` | `DrumTomHigh`、`Item140` | 共享通用语义 |
| `Item_141` | `DrumTomLow`、`Item141` | 共享通用语义 |
| `Item_142` | `DrumTomMid`、`Item142` | 共享通用语义 |
| `Item_143` | `DrumClosedHiHat`、`Item143` | 共享通用语义 |
| `Item_144` | `DrumCymbal1`、`Item144` | 共享通用语义 |
| `Item_145` | `DrumCymbal2`、`Item145` | 共享通用语义 |
| `Item_146` | `DrumKick`、`Item146` | 共享通用语义 |
| `Item_147` | `DrumTamaSnare`、`Item147` | 共享通用语义 |
| `Item_148` | `DrumFloorTom`、`Item148` | 共享通用语义 |
| `Item_149` | `Item149` | 准确语义 |
| `Item_15` | `Item15` | 共享通用语义 |
| `Item_150` | `Item150` | 准确语义 |
| `Item_151` | `Item151` | 准确语义 |
| `Item_152` | `Item152` | 共享通用语义 |
| `Item_153` | `Item153` | 共享通用语义 |
| `Item_154` | `Item154` | 共享通用语义 |
| `Item_155` | `Item155` | 共享通用语义 |
| `Item_156` | `Item156` | 准确语义 |
| `Item_157` | `Item157` | 共享通用语义 |
| `Item_158` | `Item158` | 共享通用语义 |
| `Item_159` | `Item159` | 共享通用语义 |
| `Item_16` | `Item16` | 共享通用语义 |
| `Item_160` | `Item160` | 共享通用语义 |
| `Item_161` | `Item161` | 共享通用语义 |
| `Item_162` | `Item162` | 共享通用语义 |
| `Item_163` | `Item163` | 共享通用语义 |
| `Item_164` | `Item164` | 共享通用语义 |
| `Item_165` | `Item165` | 共享通用语义 |
| `Item_166` | `Item166` | 准确语义 |
| `Item_167` | `Item167` | 共享通用语义 |
| `Item_168` | `Item168` | 准确语义 |
| `Item_169` | `Item169` | 共享通用语义 |
| `Item_17` | `Item17` | 共享通用语义 |
| `Item_170` | `Item170` | 共享通用语义 |
| `Item_171` | `Item171` | 共享通用语义 |
| `Item_172` | `Item172` | 共享通用语义 |
| `Item_173` | `Item173` | 共享通用语义 |
| `Item_174` | `Item174` | 共享通用语义 |
| `Item_175` | `Item175` | 准确语义 |
| `Item_176` | `Item176` | 共享通用语义 |
| `Item_177` | `Item177` | 共享通用语义 |
| `Item_178` | `Item178` | 共享通用语义 |
| `Item_179` | — | 准确语义 · 使用炼金烧瓶 · `AlchemyFlask` |
| `Item_18` | `Item1`、`Item18` | 共享通用语义 |
| `Item_180` | — | 共享通用语义 · 武器发射声 · `WeaponDischarge` |
| `Item_181` | — | 共享通用语义 · 喷射声 · `Spray` |
| `Item_182` | — | 共享通用语义 · 利刃挥动 · `BladeSwing` |
| `Item_183` | — | 共享通用语义 · 枪声 · `Gunshot` |
| `Item_184` | — | 共享通用语义 · 枪声 · `Gunshot` |
| `Item_185` | — | 准确语义 · 夜之护身符发声 · `NightAmulet` |
| `Item_186` | — | 准确语义 · 神秘头骨发声 · `SlasherSkull` |
| `Item_187` | — | 共享通用语义 · 长笛声 · `Flute` |
| `Item_188` | — | 准确语义 · 狼形变身音效 · `WolfNecklace` |
| `Item_189` | — | 共享通用语义 · 工具运转声 · `ToolMotor` |
| `Item_19` | `Item1`、`Item19` | 共享通用语义 |
| `Item_190` | — | 共享通用语义 · 工具运转声 · `ToolMotor` |
| `Item_191` | — | 原版未使用 |
| `Item_192` | — | 准确语义 · 风筝线轴转动 · `KiteReel` |
| `Item_193` | — | 准确语义 · 电鳗发声 · `EelSound` |
| `Item_194` | — | 原版未使用 |
| `Item_195` | — | 原版未使用 |
| `Item_196` | — | 准确语义 · 木棍独角兽音效 · `StickUnicorn` |
| `Item_197` | — | 共享通用语义 · 观战宝珠音效 · `SpectateOrb` |
| `Item_198` | — | 共享通用语义 · 观战宝珠音效 · `SpectateOrb` |
| `Item_199` | — | 准确语义 · 碎岩龟钻动 · `DigtoiseDrill` |
| `Item_2` | `Item2` | 共享通用语义 |
| `Item_20` | `Item20` | 共享通用语义 |
| `Item_200` | — | 原版未使用 |
| `Item_201` | — | 准确语义 · 冰川之牙发射 · `IceGunshot` |
| `Item_202` | — | 原版未使用 |
| `Item_203` | — | 准确语义 · 电弧涌动 · `ElectricArc` |
| `Item_204` | — | 准确语义 · 闪电击中 · `LightningHit` |
| `Item_205` | — | 共享通用语义 · 魔法脉冲 · `MagicPulse` |
| `Item_206` | — | 准确语义 · 火山岩音效 · `VolcanicStone` |
| `Item_207` | — | 准确语义 · 毁灭臂环发声 · `ArmletSound` |
| `Item_208` | — | 原版未使用 |
| `Item_209` | — | 原版未使用 |
| `Item_21` | `Item21` | 共享通用语义 |
| `Item_210` | — | 准确语义 · 冰川之牙蓄力 · `IceCharge` |
| `Item_22` | `Item22` | 原版未使用 |
| `Item_23` | `Item23` | 共享通用语义 |
| `Item_24` | `Item24` | 共享通用语义 |
| `Item_25` | `Item25` | 共享通用语义 |
| `Item_26` | `Item26` | 共享通用语义 |
| `Item_27` | `Item27` | 共享通用语义 |
| `Item_28` | `Item28` | 共享通用语义 |
| `Item_29` | `Item29` | 准确语义 |
| `Item_3` | `Item3` | 共享通用语义 |
| `Item_30` | `Item30` | 准确语义 |
| `Item_31` | `Item31` | 共享通用语义 |
| `Item_32` | `Item32` | 共享通用语义 |
| `Item_33` | `Item33` | 共享通用语义 |
| `Item_34` | `Item34` | 共享通用语义 |
| `Item_35` | `Item35` | 准确语义 |
| `Item_36` | `Item36` | 共享通用语义 |
| `Item_37` | `Item37` | 共享通用语义 |
| `Item_38` | `Item38` | 共享通用语义 |
| `Item_39` | `Item39` | 共享通用语义 |
| `Item_4` | `Item4` | 共享通用语义 |
| `Item_40` | `Item40` | 共享通用语义 |
| `Item_41` | `Item41` | 共享通用语义 |
| `Item_42` | `Item42` | 共享通用语义 |
| `Item_43` | `Item43` | 共享通用语义 |
| `Item_44` | `Item44` | 共享通用语义 |
| `Item_45` | `Item45` | 共享通用语义 |
| `Item_46` | `Item46` | 共享通用语义 |
| `Item_47` | `Item47` | 共享通用语义 |
| `Item_48` | `Item48` | 共享通用语义 |
| `Item_49` | `Item49` | 共享通用语义 |
| `Item_5` | `Item5` | 共享通用语义 |
| `Item_50` | `Item50` | 准确语义 |
| `Item_51` | `Item51` | 准确语义 |
| `Item_52` | `Item52` | 准确语义 |
| `Item_53` | `Item53` | 准确语义 |
| `Item_54` | `Item54` | 共享通用语义 |
| `Item_55` | `Item55` | 准确语义 |
| `Item_56` | `Item56` | 准确语义 |
| `Item_57` | `Item57`、`Meowmere` | 共享通用语义 |
| `Item_58` | `Item58`、`Meowmere` | 共享通用语义 |
| `Item_59` | `Item59` | 共享通用语义 |
| `Item_6` | `Item6` | 共享通用语义 |
| `Item_60` | `Item60` | 共享通用语义 |
| `Item_61` | `Item61` | 共享通用语义 |
| `Item_62` | `Item62` | 共享通用语义 |
| `Item_63` | `Item63` | 准确语义 |
| `Item_64` | `Item64` | 共享通用语义 |
| `Item_65` | `Item65` | 原版未使用 |
| `Item_66` | `Item66` | 共享通用语义 |
| `Item_67` | `Item67` | 共享通用语义 |
| `Item_68` | `Item68` | 原版未使用 |
| `Item_69` | `Item69` | 共享通用语义 |
| `Item_7` | `Item7` | 共享通用语义 |
| `Item_70` | `Item70` | 准确语义 |
| `Item_71` | `Item71` | 共享通用语义 |
| `Item_72` | `Item72` | 共享通用语义 |
| `Item_73` | `Item73` | 共享通用语义 |
| `Item_74` | `Item74` | 共享通用语义 |
| `Item_75` | `Item75` | 共享通用语义 |
| `Item_76` | `Item76` | 共享通用语义 |
| `Item_77` | `Item77` | 共享通用语义 |
| `Item_78` | `Item78` | 共享通用语义 |
| `Item_79` | `Item79` | 共享通用语义 |
| `Item_8` | `Item8` | 共享通用语义 |
| `Item_80` | `Item80` | 共享通用语义 |
| `Item_81` | `Item81` | 共享通用语义 |
| `Item_82` | `Item82` | 共享通用语义 |
| `Item_83` | `Item83` | 共享通用语义 |
| `Item_84` | `Item84` | 共享通用语义 |
| `Item_85` | `Item85` | 共享通用语义 |
| `Item_86` | `Item86` | 原版未使用 |
| `Item_87` | `Item87` | 原版未使用 |
| `Item_88` | `Item88` | 共享通用语义 |
| `Item_89` | `Item89` | 准确语义 |
| `Item_9` | `Item9` | 共享通用语义 |
| `Item_90` | `Item90` | 准确语义 |
| `Item_91` | `Item91` | 共享通用语义 |
| `Item_92` | `Item92` | 共享通用语义 |
| `Item_93` | `Item93` | 原版未使用 |
| `Item_94` | `Item94` | 共享通用语义 |
| `Item_95` | `Item95` | 准确语义 |
| `Item_96` | `Item96` | 共享通用语义 |
| `Item_97` | `Item97` | 共享通用语义 |
| `Item_98` | `Item98` | 共享通用语义 |
| `Item_99` | `Item99` | 共享通用语义 |
| `Liquid_0` | `Waterfall` | 准确语义 |
| `Liquid_1` | `Lavafall` | 准确语义 |
| `MaxMana` | `MaxMana` | 共享通用语义 |
| `Mech_0` | `Mech` | 共享通用语义 |
| `Menu_Close` | `MenuClose` | 准确语义 |
| `Menu_Open` | `MenuOpen` | 准确语义 |
| `Menu_Tick` | `MenuTick` | 共享通用语义 |
| `NPC_Hit_1` | `NPCHit1` | 共享通用语义 |
| `NPC_Hit_10` | `NPCHit10` | 共享通用语义 |
| `NPC_Hit_11` | `NPCHit11` | 共享通用语义 |
| `NPC_Hit_12` | `NPCHit12` | 共享通用语义 |
| `NPC_Hit_13` | `NPCHit13` | 共享通用语义 |
| `NPC_Hit_14` | `NPCHit14` | 共享通用语义 |
| `NPC_Hit_15` | `NPCHit15` | 准确语义 |
| `NPC_Hit_16` | `NPCHit16` | 准确语义 |
| `NPC_Hit_17` | `NPCHit17` | 准确语义 |
| `NPC_Hit_18` | `NPCHit18` | 共享通用语义 |
| `NPC_Hit_19` | `NPCHit19` | 共享通用语义 |
| `NPC_Hit_2` | `NPCHit2` | 共享通用语义 |
| `NPC_Hit_20` | `NPCHit20` | 共享通用语义 |
| `NPC_Hit_21` | `NPCHit21` | 共享通用语义 |
| `NPC_Hit_22` | `NPCHit22` | 共享通用语义 |
| `NPC_Hit_23` | `NPCHit23` | 共享通用语义 |
| `NPC_Hit_24` | `NPCHit24` | 共享通用语义 |
| `NPC_Hit_25` | `NPCHit25` | 共享通用语义 |
| `NPC_Hit_26` | `NPCHit26` | 共享通用语义 |
| `NPC_Hit_27` | `NPCHit27` | 共享通用语义 |
| `NPC_Hit_28` | `NPCHit28` | 共享通用语义 |
| `NPC_Hit_29` | `NPCHit29` | 共享通用语义 |
| `NPC_Hit_3` | `NPCHit3` | 共享通用语义 |
| `NPC_Hit_30` | `NPCHit30` | 共享通用语义 |
| `NPC_Hit_31` | `NPCHit31` | 共享通用语义 |
| `NPC_Hit_32` | `NPCHit32` | 共享通用语义 |
| `NPC_Hit_33` | `NPCHit33` | 共享通用语义 |
| `NPC_Hit_34` | `NPCHit34` | 共享通用语义 |
| `NPC_Hit_35` | `NPCHit35` | 共享通用语义 |
| `NPC_Hit_36` | `NPCHit36` | 共享通用语义 |
| `NPC_Hit_37` | `NPCHit37` | 共享通用语义 |
| `NPC_Hit_38` | `NPCHit38` | 共享通用语义 |
| `NPC_Hit_39` | `NPCHit39` | 共享通用语义 |
| `NPC_Hit_4` | `NPCHit4` | 共享通用语义 |
| `NPC_Hit_40` | `NPCHit40` | 共享通用语义 |
| `NPC_Hit_41` | `NPCHit41` | 共享通用语义 |
| `NPC_Hit_42` | `NPCHit42` | 共享通用语义 |
| `NPC_Hit_43` | `NPCHit43` | 共享通用语义 |
| `NPC_Hit_44` | `NPCHit44` | 共享通用语义 |
| `NPC_Hit_45` | `NPCHit45` | 共享通用语义 |
| `NPC_Hit_46` | `NPCHit46` | 共享通用语义 |
| `NPC_Hit_47` | `NPCHit47` | 共享通用语义 |
| `NPC_Hit_48` | `NPCHit48` | 共享通用语义 |
| `NPC_Hit_49` | `NPCHit49` | 共享通用语义 |
| `NPC_Hit_5` | `NPCHit5` | 共享通用语义 |
| `NPC_Hit_50` | `NPCHit50` | 共享通用语义 |
| `NPC_Hit_51` | `NPCHit51` | 共享通用语义 |
| `NPC_Hit_52` | `NPCHit52` | 共享通用语义 |
| `NPC_Hit_53` | `NPCHit53` | 共享通用语义 |
| `NPC_Hit_54` | `NPCHit54` | 共享通用语义 |
| `NPC_Hit_55` | `NPCHit55` | 共享通用语义 |
| `NPC_Hit_56` | `NPCHit56` | 共享通用语义 |
| `NPC_Hit_57` | `NPCHit57` | 共享通用语义 |
| `NPC_Hit_58` | — | 准确语义 · 雕像怪受击 · `StatueHit` |
| `NPC_Hit_6` | `NPCHit6` | 共享通用语义 |
| `NPC_Hit_7` | `NPCHit7` | 共享通用语义 |
| `NPC_Hit_8` | `NPCHit8` | 共享通用语义 |
| `NPC_Hit_9` | `NPCHit9` | 共享通用语义 |
| `NPC_Killed_1` | `NPCDeath1` | 共享通用语义 |
| `NPC_Killed_10` | `MoonLord`、`NPCDeath10` | 共享通用语义 |
| `NPC_Killed_11` | `NPCDeath11` | 共享通用语义 |
| `NPC_Killed_12` | `NPCDeath12` | 共享通用语义 |
| `NPC_Killed_13` | `NPCDeath13` | 共享通用语义 |
| `NPC_Killed_14` | `DD2_GoblinBomb`、`NPCDeath14` | 共享通用语义 |
| `NPC_Killed_15` | `NPCDeath15` | 共享通用语义 |
| `NPC_Killed_16` | `NPCDeath16` | 共享通用语义 |
| `NPC_Killed_17` | `NPCDeath17` | 共享通用语义 |
| `NPC_Killed_18` | `NPCDeath18` | 共享通用语义 |
| `NPC_Killed_19` | `NPCDeath19` | 共享通用语义 |
| `NPC_Killed_2` | `NPCDeath2` | 共享通用语义 |
| `NPC_Killed_20` | `NPCDeath20` | 共享通用语义 |
| `NPC_Killed_21` | `NPCDeath21` | 共享通用语义 |
| `NPC_Killed_22` | `NPCDeath22` | 共享通用语义 |
| `NPC_Killed_23` | `NPCDeath23` | 共享通用语义 |
| `NPC_Killed_24` | `NPCDeath24` | 共享通用语义 |
| `NPC_Killed_25` | `NPCDeath25` | 共享通用语义 |
| `NPC_Killed_26` | `NPCDeath26` | 共享通用语义 |
| `NPC_Killed_27` | `NPCDeath27` | 共享通用语义 |
| `NPC_Killed_28` | `NPCDeath28` | 共享通用语义 |
| `NPC_Killed_29` | `NPCDeath29` | 共享通用语义 |
| `NPC_Killed_3` | `NPCDeath3` | 共享通用语义 |
| `NPC_Killed_30` | `NPCDeath30` | 共享通用语义 |
| `NPC_Killed_31` | `NPCDeath31` | 共享通用语义 |
| `NPC_Killed_32` | `NPCDeath32` | 共享通用语义 |
| `NPC_Killed_33` | `NPCDeath33` | 共享通用语义 |
| `NPC_Killed_34` | `NPCDeath34` | 共享通用语义 |
| `NPC_Killed_35` | `NPCDeath35` | 共享通用语义 |
| `NPC_Killed_36` | `NPCDeath36` | 共享通用语义 |
| `NPC_Killed_37` | `NPCDeath37` | 共享通用语义 |
| `NPC_Killed_38` | `NPCDeath38` | 共享通用语义 |
| `NPC_Killed_39` | `NPCDeath39` | 共享通用语义 |
| `NPC_Killed_4` | `NPCDeath4` | 共享通用语义 |
| `NPC_Killed_40` | `NPCDeath40` | 共享通用语义 |
| `NPC_Killed_41` | `NPCDeath41` | 共享通用语义 |
| `NPC_Killed_42` | `NPCDeath42` | 共享通用语义 |
| `NPC_Killed_43` | `NPCDeath43` | 共享通用语义 |
| `NPC_Killed_44` | `NPCDeath44` | 共享通用语义 |
| `NPC_Killed_45` | `NPCDeath45` | 共享通用语义 |
| `NPC_Killed_46` | `NPCDeath46` | 共享通用语义 |
| `NPC_Killed_47` | `NPCDeath47` | 共享通用语义 |
| `NPC_Killed_48` | `NPCDeath48` | 共享通用语义 |
| `NPC_Killed_49` | `NPCDeath49` | 共享通用语义 |
| `NPC_Killed_5` | `NPCDeath5` | 共享通用语义 |
| `NPC_Killed_50` | `NPCDeath50` | 共享通用语义 |
| `NPC_Killed_51` | `NPCDeath51` | 共享通用语义 |
| `NPC_Killed_52` | `NPCDeath52` | 共享通用语义 |
| `NPC_Killed_53` | `NPCDeath53` | 共享通用语义 |
| `NPC_Killed_54` | `NPCDeath54` | 共享通用语义 |
| `NPC_Killed_55` | `NPCDeath55` | 共享通用语义 |
| `NPC_Killed_56` | `NPCDeath56` | 共享通用语义 |
| `NPC_Killed_57` | `NPCDeath57` | 共享通用语义 |
| `NPC_Killed_58` | `NPCDeath58` | 准确语义 |
| `NPC_Killed_59` | `NPCDeath59` | 共享通用语义 |
| `NPC_Killed_6` | `NPCDeath6` | 共享通用语义 |
| `NPC_Killed_60` | `NPCDeath60` | 共享通用语义 |
| `NPC_Killed_61` | `NPCDeath61` | 共享通用语义 |
| `NPC_Killed_62` | `NPCDeath62` | 共享通用语义 |
| `NPC_Killed_63` | `NPCDeath63` | 共享通用语义 |
| `NPC_Killed_64` | `NPCDeath64` | 共享通用语义 |
| `NPC_Killed_65` | `NPCDeath65` | 共享通用语义 |
| `NPC_Killed_66` | `NPCDeath66` | 共享通用语义 |
| `NPC_Killed_67` | — | 准确语义 · 雕像怪倒下 · `StatueDeath` |
| `NPC_Killed_68` | — | 原版未使用 |
| `NPC_Killed_7` | `NPCDeath7` | 共享通用语义 |
| `NPC_Killed_8` | `NPCDeath8` | 共享通用语义 |
| `NPC_Killed_9` | `NPCDeath9` | 共享通用语义 |
| `Pixie` | `Pixie` | 准确语义 |
| `Player_Hit_0` | `PlayerHit` | 共享通用语义 |
| `Player_Hit_1` | `PlayerHit` | 共享通用语义 |
| `Player_Hit_2` | `PlayerHit` | 共享通用语义 |
| `Player_Killed` | `PlayerKilled` | 准确语义 |
| `Research_0` | `ResearchComplete` | 准确语义 |
| `Research_1` | `Research` | 准确语义 |
| `Research_2` | `Research` | 准确语义 |
| `Research_3` | `Research` | 准确语义 |
| `Roar_0` | `ForceRoar`、`ForceRoarPitched`、`Roar` | 共享通用语义 |
| `Roar_1` | `WormDig`、`WormDigQuiet` | 共享通用语义 |
| `Roar_2` | `ScaryScream` | 共享通用语义 |
| `Run` | `Run` | 共享通用语义 |
| `Shatter` | `Shatter` | 共享通用语义 |
| `Splash_0` | `Splash` | 共享通用语义 |
| `Splash_1` | `SplashWeak` | 共享通用语义 |
| `Splash_2` | `Shimmer1` | 共享通用语义 |
| `Splash_3` | `Shimmer2` | 准确语义 |
| `Splash_4` | `ShimmerWeak1` | 准确语义 |
| `Splash_5` | `ShimmerWeak2` | 共享通用语义 |
| `Thunder_0` | `Thunder` | 准确语义 |
| `Thunder_1` | `Thunder` | 准确语义 |
| `Thunder_2` | `Thunder` | 准确语义 |
| `Thunder_3` | `Thunder` | 准确语义 |
| `Thunder_4` | `Thunder` | 准确语义 |
| `Thunder_5` | `Thunder` | 准确语义 |
| `Tink_0` | `Tink` | 共享通用语义 |
| `Tink_1` | `Tink` | 共享通用语义 |
| `Tink_2` | `Tink` | 共享通用语义 |
| `Unlock` | `Unlock` | 准确语义 |
| `Zombie_0` | `ZombieMoan` | 共享通用语义 |
| `Zombie_1` | `Zombie1`、`ZombieMoan` | 共享通用语义 |
| `Zombie_10` | `Duck`、`Zombie10` | 准确语义 |
| `Zombie_100` | `Zombie100` | 共享通用语义 |
| `Zombie_101` | `Zombie101` | 共享通用语义 |
| `Zombie_102` | `Zombie102` | 准确语义 |
| `Zombie_103` | `Zombie103` | 准确语义 |
| `Zombie_104` | `Zombie104` | 准确语义 |
| `Zombie_105` | `Zombie105` | 准确语义 |
| `Zombie_106` | `Seagull`、`Zombie106` | 准确语义 |
| `Zombie_107` | `Seagull`、`Zombie107` | 准确语义 |
| `Zombie_108` | `Seagull`、`Zombie108` | 准确语义 |
| `Zombie_109` | `Dolphin`、`Zombie109` | 准确语义 |
| `Zombie_11` | `Duck`、`Zombie11` | 准确语义 |
| `Zombie_110` | `Owl`、`Zombie110` | 准确语义 |
| `Zombie_111` | `Owl`、`Zombie111` | 准确语义 |
| `Zombie_112` | `Owl`、`Zombie112` | 准确语义 |
| `Zombie_113` | `Owl`、`Zombie113` | 准确语义 |
| `Zombie_114` | `Owl`、`Zombie114` | 准确语义 |
| `Zombie_115` | `QueenSlime`、`Zombie115` | 共享通用语义 |
| `Zombie_116` | `QueenSlime`、`Zombie116` | 共享通用语义 |
| `Zombie_117` | `QueenSlime`、`Zombie117` | 共享通用语义 |
| `Zombie_118` | `Cockatiel`、`Zombie118` | 准确语义 |
| `Zombie_119` | `Cockatiel`、`Zombie119` | 准确语义 |
| `Zombie_12` | `Duck`、`Zombie12` | 准确语义 |
| `Zombie_120` | `Cockatiel`、`Zombie120` | 准确语义 |
| `Zombie_121` | `Clown`、`Zombie121` | 准确语义 |
| `Zombie_122` | `Clown`、`Zombie122` | 准确语义 |
| `Zombie_123` | `Clown`、`Zombie123` | 准确语义 |
| `Zombie_124` | `Zombie124` | 准确语义 |
| `Zombie_125` | `Zombie125` | 准确语义 |
| `Zombie_126` | `Macaw`、`Zombie126` | 准确语义 |
| `Zombie_127` | `Macaw`、`Zombie127` | 准确语义 |
| `Zombie_128` | `Macaw`、`Zombie128` | 准确语义 |
| `Zombie_129` | `Toucan`、`Zombie129` | 准确语义 |
| `Zombie_13` | `Frog`、`Zombie13` | 准确语义 |
| `Zombie_130` | `Toucan`、`Zombie130` | 准确语义 |
| `Zombie_14` | `Bird`、`Bird14`、`Zombie14` | 共享通用语义 |
| `Zombie_15` | `Critter`、`Zombie15` | 共享通用语义 |
| `Zombie_16` | `Bird`、`Bird16`、`Zombie16` | 共享通用语义 |
| `Zombie_17` | `Bird`、`Bird17`、`Zombie17` | 共享通用语义 |
| `Zombie_18` | `Bird`、`Bird18`、`Zombie18` | 共享通用语义 |
| `Zombie_19` | `Bird`、`Bird19`、`Zombie19` | 共享通用语义 |
| `Zombie_2` | `Zombie2`、`ZombieMoan` | 共享通用语义 |
| `Zombie_20` | `Zombie20` | 准确语义 |
| `Zombie_21` | `BloodZombie`、`Zombie21` | 共享通用语义 |
| `Zombie_22` | `BloodZombie`、`Zombie22` | 共享通用语义 |
| `Zombie_23` | `BloodZombie`、`Zombie23` | 共享通用语义 |
| `Zombie_24` | `Zombie24` | 准确语义 |
| `Zombie_25` | `Zombie25` | 准确语义 |
| `Zombie_26` | `Zombie26` | 共享通用语义 |
| `Zombie_27` | `Zombie27` | 共享通用语义 |
| `Zombie_28` | `Zombie28` | 共享通用语义 |
| `Zombie_29` | `Zombie29` | 共享通用语义 |
| `Zombie_3` | `Mummy`、`Zombie3` | 准确语义 |
| `Zombie_30` | `Zombie30` | 准确语义 |
| `Zombie_31` | `Zombie31` | 准确语义 |
| `Zombie_32` | `Zombie32` | 准确语义 |
| `Zombie_33` | `Zombie33` | 准确语义 |
| `Zombie_34` | `Zombie34` | 共享通用语义 |
| `Zombie_35` | `Zombie35` | 共享通用语义 |
| `Zombie_36` | `Zombie36` | 准确语义 |
| `Zombie_37` | `Zombie37` | 准确语义 |
| `Zombie_38` | `Zombie38` | 准确语义 |
| `Zombie_39` | `Zombie39` | 准确语义 |
| `Zombie_4` | `Mummy`、`Zombie4` | 准确语义 |
| `Zombie_40` | `Zombie40` | 准确语义 |
| `Zombie_41` | `Zombie41` | 准确语义 |
| `Zombie_42` | `Zombie42` | 准确语义 |
| `Zombie_43` | `Zombie43` | 准确语义 |
| `Zombie_44` | `Zombie44` | 共享通用语义 |
| `Zombie_45` | `Zombie45` | 共享通用语义 |
| `Zombie_46` | `Zombie46` | 共享通用语义 |
| `Zombie_47` | `Zombie47` | 准确语义 |
| `Zombie_48` | `Zombie48` | 准确语义 |
| `Zombie_49` | `Zombie49` | 准确语义 |
| `Zombie_5` | `Zombie5` | 原版未使用 |
| `Zombie_50` | `Zombie50` | 准确语义 |
| `Zombie_51` | `Zombie51` | 准确语义 |
| `Zombie_52` | `Zombie52` | 准确语义 |
| `Zombie_53` | `Zombie53` | 共享通用语义 |
| `Zombie_54` | `Zombie54` | 共享通用语义 |
| `Zombie_55` | `Zombie55` | 共享通用语义 |
| `Zombie_56` | `Zombie56` | 共享通用语义 |
| `Zombie_57` | `Zombie57` | 准确语义 |
| `Zombie_58` | `Zombie58` | 准确语义 |
| `Zombie_59` | `Zombie59` | 准确语义 |
| `Zombie_6` | `Zombie6` | 准确语义 |
| `Zombie_60` | `Zombie60` | 准确语义 |
| `Zombie_61` | `Zombie61` | 准确语义 |
| `Zombie_62` | `Zombie62` | 准确语义 |
| `Zombie_63` | `Zombie63` | 准确语义 |
| `Zombie_64` | `Zombie64` | 准确语义 |
| `Zombie_65` | `Zombie65` | 准确语义 |
| `Zombie_66` | `Zombie66` | 共享通用语义 |
| `Zombie_67` | `Zombie67` | 共享通用语义 |
| `Zombie_68` | `Zombie68` | 共享通用语义 |
| `Zombie_69` | `Zombie69` | 共享通用语义 |
| `Zombie_7` | `SandShark`、`Zombie7` | 共享通用语义 |
| `Zombie_70` | `Zombie70` | 共享通用语义 |
| `Zombie_71` | `Zombie71` | 共享通用语义 |
| `Zombie_72` | `Zombie72` | 共享通用语义 |
| `Zombie_73` | `Zombie73` | 准确语义 |
| `Zombie_74` | `Zombie74` | 准确语义 |
| `Zombie_75` | `Zombie75` | 准确语义 |
| `Zombie_76` | `Zombie76` | 准确语义 |
| `Zombie_77` | `Zombie77` | 准确语义 |
| `Zombie_78` | `Zombie78` | 准确语义 |
| `Zombie_79` | `Zombie79` | 准确语义 |
| `Zombie_8` | `Zombie8` | 准确语义 |
| `Zombie_80` | `Zombie80` | 准确语义 |
| `Zombie_81` | `Zombie81` | 共享通用语义 |
| `Zombie_82` | `Zombie82` | 共享通用语义 |
| `Zombie_83` | `Zombie83` | 共享通用语义 |
| `Zombie_84` | `Zombie84` | 准确语义 |
| `Zombie_85` | `Zombie85` | 准确语义 |
| `Zombie_86` | `Zombie86` | 准确语义 |
| `Zombie_87` | `Zombie87` | 原版未使用 |
| `Zombie_88` | `Zombie88` | 准确语义 |
| `Zombie_89` | `Zombie89` | 准确语义 |
| `Zombie_9` | `Zombie9` | 准确语义 |
| `Zombie_90` | `Zombie90` | 准确语义 |
| `Zombie_91` | `Zombie91` | 准确语义 |
| `Zombie_92` | `Zombie92` | 准确语义 |
| `Zombie_93` | `Zombie93` | 共享通用语义 |
| `Zombie_94` | `Zombie94` | 共享通用语义 |
| `Zombie_95` | `Zombie95` | 共享通用语义 |
| `Zombie_96` | `Zombie96` | 共享通用语义 |
| `Zombie_97` | `Zombie97` | 共享通用语义 |
| `Zombie_98` | `Zombie98` | 共享通用语义 |
| `Zombie_99` | `Zombie99` | 共享通用语义 |

## 原版背景音乐

稳定键为 Terraria/Music/ 加表中的 MusicID 名称。槽位 28 是雨声环境音，45 和 51 没有注册曲目名。资源包替换同一槽位的音频时沿用原曲名，可用显式注册覆盖标题。

| 槽位 | MusicID | 中文 | 英文 |
| ---: | --- | --- | --- |
| 1 | `OverworldDay` | 森林（日间） | Overworld Day |
| 2 | `Eerie` | 诡异 | Eerie |
| 3 | `Night` | 森林（夜间） | Night |
| 4 | `Underground` | 地下 | Underground |
| 5 | `Boss1` | 首领 1 | Boss 1 |
| 6 | `Title` | 标题画面 | Title Screen |
| 7 | `Jungle` | 丛林 | Jungle |
| 8 | `Corruption` | 腐化之地 | Corruption |
| 9 | `TheHallow` | 神圣之地 | The Hallow |
| 10 | `UndergroundCorruption` | 地下腐化之地 | Underground Corruption |
| 11 | `UndergroundHallow` | 地下神圣之地 | Underground Hallow |
| 12 | `Boss2` | 首领 2 | Boss 2 |
| 13 | `Boss3` | 首领 3 | Boss 3 |
| 14 | `Snow` | 雪原 | Snow |
| 15 | `Space` | 太空（夜间） | Space Night |
| 16 | `Crimson` | 猩红之地 | Crimson |
| 17 | `Boss4` | 首领 4 | Boss 4 |
| 18 | `AltOverworldDay` | 森林（日间变奏） | Alternate Overworld Day |
| 19 | `Rain` | 雨天 | Rain |
| 20 | `Ice` | 地下雪原 | Ice |
| 21 | `Desert` | 沙漠 | Desert |
| 22 | `Ocean` | 海洋（日间） | Ocean Day |
| 23 | `Dungeon` | 地牢 | Dungeon |
| 24 | `Plantera` | 世纪之花 | Plantera |
| 25 | `Boss5` | 首领 5 | Boss 5 |
| 26 | `Temple` | 丛林神庙 | Jungle Temple |
| 27 | `Eclipse` | 日食 | Solar Eclipse |
| 29 | `Mushrooms` | 发光蘑菇生物群落 | Mushroom Biome |
| 30 | `PumpkinMoon` | 南瓜月 | Pumpkin Moon |
| 31 | `AltUnderground` | 地下（变奏） | Alternate Underground |
| 32 | `FrostMoon` | 霜月 | Frost Moon |
| 33 | `UndergroundCrimson` | 地下猩红之地 | Underground Crimson |
| 34 | `TheTowers` | 天界柱 | Lunar Pillars |
| 35 | `PirateInvasion` | 海盗入侵 | Pirate Invasion |
| 36 | `Hell` | 地狱 | Underworld |
| 37 | `MartianMadness` | 火星暴乱 | Martian Madness |
| 38 | `LunarBoss` | 月亮领主 | Moon Lord |
| 39 | `GoblinInvasion` | 哥布林入侵 | Goblin Invasion |
| 40 | `Sandstorm` | 沙尘暴 | Sandstorm |
| 41 | `OldOnesArmy` | 撒旦军队 | Old One's Army |
| 42 | `SpaceDay` | 太空（日间） | Space Day |
| 43 | `OceanNight` | 海洋（夜间） | Ocean Night |
| 44 | `WindyDay` | 大风天 | Windy Day |
| 46 | `TownDay` | 城镇（日间） | Town Day |
| 47 | `TownNight` | 城镇（夜间） | Town Night |
| 48 | `SlimeRain` | 史莱姆雨 | Slime Rain |
| 49 | `DayRemix` | 森林（日间混音） | Overworld Day Remix |
| 50 | `MenuMusic` | 旅途的开始（含前奏） | Journey's Beginning (with intro) |
| 52 | `Monsoon` | 暴风雨 | Storm |
| 53 | `Graveyard` | 墓地 | Graveyard |
| 54 | `JungleUnderground` | 地下丛林 | Underground Jungle |
| 55 | `JungleNight` | 丛林（夜间） | Jungle Night |
| 56 | `QueenSlime` | 史莱姆皇后 | Queen Slime |
| 57 | `EmpressOfLight` | 光之女皇 | Empress of Light |
| 58 | `DukeFishron` | 猪龙鱼公爵 | Duke Fishron |
| 59 | `MorningRain` | 晨雨 | Morning Rain |
| 60 | `ConsoleMenu` | 主机版主菜单 | Console Menu |
| 61 | `UndergroundDesert` | 地下沙漠 | Underground Desert |
| 62 | `OtherworldlyRain` | 异界 · 雨天 | Otherworldly Rain |
| 63 | `OtherworldlyDay` | 异界 · 日间 | Otherworldly Day |
| 64 | `OtherworldlyNight` | 异界 · 夜间 | Otherworldly Night |
| 65 | `OtherworldlyUnderground` | 异界 · 地下 | Otherworldly Underground |
| 66 | `OtherworldlyDesert` | 异界 · 沙漠 | Otherworldly Desert |
| 67 | `OtherworldlyOcean` | 异界 · 海洋 | Otherworldly Ocean |
| 68 | `OtherworldlyMushrooms` | 异界 · 发光蘑菇生物群落 | Otherworldly Mushrooms |
| 69 | `OtherworldlyDungeon` | 异界 · 地牢 | Otherworldly Dungeon |
| 70 | `OtherworldlySpace` | 异界 · 太空 | Otherworldly Space |
| 71 | `OtherworldlyUnderworld` | 异界 · 地狱 | Otherworldly Underworld |
| 72 | `OtherworldlySnow` | 异界 · 雪原 | Otherworldly Snow |
| 73 | `OtherworldlyCorruption` | 异界 · 腐化之地 | Otherworldly Corruption |
| 74 | `OtherworldlyUGCorrption` | 异界 · 地下腐化之地 | Otherworldly Underground Corruption |
| 75 | `OtherworldlyCrimson` | 异界 · 猩红之地 | Otherworldly Crimson |
| 76 | `OtherworldlyUGCrimson` | 异界 · 地下猩红之地 | Otherworldly Underground Crimson |
| 77 | `OtherworldlyIce` | 异界 · 地下雪原 | Otherworldly Ice |
| 78 | `OtherworldlyUGHallow` | 异界 · 地下神圣之地 | Otherworldly Underground Hallow |
| 79 | `OtherworldlyEerie` | 异界 · 诡异 | Otherworldly Eerie |
| 80 | `OtherworldlyBoss2` | 异界 · 首领 2 | Otherworldly Boss 2 |
| 81 | `OtherworldlyBoss1` | 异界 · 首领 1 | Otherworldly Boss 1 |
| 82 | `OtherworldlyInvasion` | 异界 · 入侵 | Otherworldly Invasion |
| 83 | `OtherworldlyTowers` | 异界 · 天界柱 | Otherworldly Towers |
| 84 | `OtherworldlyLunarBoss` | 异界 · 月亮领主 | Otherworldly Moon Lord |
| 85 | `OtherworldlyPlantera` | 异界 · 世纪之花 | Otherworldly Plantera |
| 86 | `OtherworldlyJungle` | 异界 · 丛林 | Otherworldly Jungle |
| 87 | `OtherworldlyWoF` | 异界 · 血肉墙 | Otherworldly Wall of Flesh |
| 88 | `OtherworldlyHallow` | 异界 · 神圣之地 | Otherworldly Hallow |
| 89 | `Credits` | 制作人员名单 | Credits |
| 90 | `Deerclops` | 独眼巨鹿 | Deerclops |
| 91 | `Shimmer` | 微光 | Shimmer |
