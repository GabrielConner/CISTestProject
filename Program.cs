using static System.Runtime.InteropServices.JavaScript.JSType;
internal class CISFinal {

   private static List<itemType> inventory = [];
   private const string saveLoc = "./saves/save.csv";
   private const string saveFolder = "./saves/";
   class itemType {
      public string brand;
      public string product;
      public float buyPrice;
      public float sellPrice;
      public uint inStock;

      public itemType() {
         brand = "";
         product = "";
         buyPrice = 0;
         sellPrice = 0;
         inStock = 0;
      }
   };

   static void Main() {
      bool error = false;
      loadSave();
      Directory.CreateDirectory(saveFolder);

      while (true) {
         Console.Clear();

         if (error)
            Console.WriteLine("Invalid input");

         Console.WriteLine("Inventory viewer\n");
         Console.WriteLine("A) Add item");
         Console.WriteLine("B) Remove item");
         Console.WriteLine("C) View all");
         Console.WriteLine("D) Close app");

         //Get choice
         string input = Console.ReadLine() ?? "null";
         if (input == "null" || input == "" || input[0] == '\t') {
            error = true;
            continue;
         }

         error = false;
         switch (input.ToLower()[0]) {
            case 'a':
               addItem();
               continue;
            case 'b':
               removeItem();
               continue;
            case 'c':
               viewAll();
               continue;
            case 'd':
               saveFile();
               Console.Clear();
               return;
         }
      }
   }


   /// <summary>
   /// Outputs all elements in <c>inventory</c> to screen
   /// </summary>
   static void viewAll() {
      Console.Clear();
      for (int i = 0; i < inventory.Count; i++) {
         Console.WriteLine($"Item {i}\t:\n\tBrand\t\t:\t{inventory[i].brand}\n\tProduct\t\t:\t" +
            $"{inventory[i].product}\n\tBuy Price\t:\t{inventory[i].buyPrice}\n\t" +
            $"Sell Price\t:\t{inventory[i].sellPrice}\n\tIn Stock\t:\t{inventory[i].inStock}\n");
      }
      Console.WriteLine("Press 'Enter' to continue...");
      Console.ReadLine();
   }


   /// <summary>
   /// Removes an item in <c>inventory</c>
   /// </summary>
   static void removeItem() {
      Console.Clear();
      
      Console.WriteLine("Press 'Enter' when ready to remove item");
      Console.WriteLine("Enter 'B' to go back");
      if ((Console.ReadLine() ?? "").ToLower() == "b")
         return;

      int index = 0;

   ask:
      do {
         Console.Clear();
         Console.WriteLine("Enter index of item to be removed");
      } while (!Int32.TryParse(Console.ReadLine() ?? "", out index));

      if (index >= inventory.Count || index < 0)
         goto ask;

      inventory.RemoveAt(index);
   }


   /// <summary>
   /// Adds an item into <c>inventory</c>
   /// </summary>
   static void addItem() {
      itemType newItem = new();
      Console.Clear();
      Console.WriteLine("Press 'Enter' when you are ready to add a new item");
      Console.WriteLine("Enter 'B' to go back");
      if ((Console.ReadLine() ?? "").ToLower() == "b")
         return;

      Console.Clear();
      Console.Write("Enter item brand\n>>");
      newItem.brand = Console.ReadLine() ?? "null";

      Console.Clear();
      Console.Write("Enter item product\n>>");
      newItem.product = Console.ReadLine() ?? "null";

      do {
         Console.Clear();
         Console.Write("Enter item buy price\n>>");
      } while (!Single.TryParse(Console.ReadLine(), out newItem.buyPrice));

      do {
         Console.Clear();
         Console.Write("Enter item sell price\n>>");
      } while (!Single.TryParse(Console.ReadLine(), out newItem.sellPrice));

      do {
         Console.Clear();
         Console.Write("Enter item stock\n>>");
      } while (!UInt32.TryParse(Console.ReadLine(), out newItem.inStock));

      Console.Clear();
      Console.WriteLine("\nPress 'Enter' to continue...");
      Console.ReadLine();

      inventory.Add(newItem);
   }


   /// <summary>
   /// Saves everything in <c>inventory</c> to a file
   /// </summary>
   /// <remarks>
   /// Save file is <c>./saves/save.csv</c>
   /// </remarks>
   static void saveFile() {
      using StreamWriter sw = File.CreateText(saveLoc);
      foreach (itemType element in inventory) {
         string line = new($"{element.brand},{element.product},{element.buyPrice},{element.sellPrice},{element.inStock}");
         sw.WriteLine(line);
      }
      sw.Flush();
   }


   /// <summary>
   /// Loads save file into <c>inventory</c>
   /// </summary>
   /// <remarks>
   /// Save file is in <c>./saves/save.csv</c>
   /// </remarks>
   static void loadSave() {
      if (!File.Exists(saveLoc))
         return;
      using StreamReader sr = File.OpenText(saveLoc);
      string line = new("");
      while (sr.Peek() > 0) {
         line = sr.ReadLine() ?? "";
         itemType val = new();
         int offset = 0;
         int length = 0;

         val.brand = line[..(offset = line.IndexOf(','))];
         length = offset;

         val.product = line[++offset..(offset = line.IndexOf(',', offset))];
         length = offset;

         val.buyPrice = Convert.ToSingle(line[++offset..(offset = line.IndexOf(',', offset))]);
         length = offset;

         val.sellPrice = Convert.ToSingle(line[++offset..(offset = line.IndexOf(',', offset))]);
         length = offset;

         val.inStock = Convert.ToUInt32(line[++offset..]);

         inventory.Add(val);
      }
   }
}