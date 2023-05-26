# DH Abilities
DH Abilities lets you set up an ability system within 5 minutes.

## Features:
- Resource System
  - Allows n-many resource types, such as mana, rage, xyz
  - Can be used in standalone without the ability system
  - Resource pools: defines one type of resource on an entity
    - User-friendly configuration via ScriptableObject
- Ability System
  - Depends on Resource System
    - Abilities can define n-many resource requirements simultanously
  - Cooldown management
  - General data configured via ScriptableObject
  - Custom data and behaviour is implemented through inheritance from Ability

### Alternative uses:
The Resource System can be used without the Ability System. Common example: stamina for running.

## Samples:
Current samples:
- Example Setup
  - Provides a minimal implementation of how this package can be used.
- Example Setup with InputSystem
  - Same as above, but using InputSystem, in case you have disabled the vanilla inputs in favor of InputSystem.
- Sample for UI using UGUI
  - Displays all abilities and cooldowns and all resource pools of the local player

## Roadmap:
Soon(tm) (in order):

- Global cooldowns/manually locking/unlocking ability usage
  - Fixes the following problem: You don't want multiple abilities to be used at once.
    - Abilities will define either a preset global cooldown that is then being applied on the AbilitySystemComponent of the local player, or for a custom, dynamic approach: just lock all ability usage alltogether until it is unlocked again.
- Allow multiple charges for abilities
  
Not so soon:
- Mirror support