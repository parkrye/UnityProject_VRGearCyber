using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public enum InteractableType { Cable, Equip, Other, None, PistolMagazine, AssaultRifleMagazine, ArrowHole }
    public enum HackProgressState { None, Progress, Failure, Success }
}
