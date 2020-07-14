

[System.Serializable]
 public class GameData
 {
     public int highest = 1;
 
     public GameData(int CompletedLVL)
     {
         if (CompletedLVL > highest) {
             highest = CompletedLVL;
         }
     }
 }
