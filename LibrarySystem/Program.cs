// See https://aka.ms/new-console-template for more information
using System;
using System.Data.Common;
using System.Runtime.InteropServices.Marshalling;

public enum ItemType    //"LibraryItem"get CS0051 默认是internal，加上public就好了
{
    Novels, Magazine, TextBook,
}

public abstract class LibraryItem
{
    private readonly string? id;
    private readonly string? title;
    private ItemType type;
    public string Id { get; }
    public string Title { get; }


    public LibraryItem(string id, string title, ItemType type)
    {
        this.Id = id;
        this.Title = title;
        this.type = type;
    }

    public abstract string GetDetails();
}

public class Novels : LibraryItem
{
    private readonly string? author;
    public Novels(string id, string title, string author)
        : base(id, title, ItemType.Novels)
    {
        this.author = author;
    }
    public override string GetDetails()
    {
        return $"Novels: {Title},Novels: {author}";
    }
}

public class Magazine : LibraryItem
{
    private readonly int? IssueNumber;
    public Magazine(string id, string title, int issueNumber)
        : base(id, title, ItemType.Magazine)
    {
        this.IssueNumber = issueNumber;
    }
    public override string GetDetails()
    {
        return $"Magazine: {Title},Magazine: {IssueNumber}";
    }
}

public class TextBook : LibraryItem
{
    private readonly string? Publisher;
    public TextBook(string id, string title, string publisher)
        : base(id, title, ItemType.TextBook)
    {
        this.Publisher = publisher;
    }
    public override string GetDetails()
    {
        return $"TextBook: {Title},TextBook: {Publisher}";
    }
}

public class Member
{
    private readonly string? name;
    public string Name { get; }
    private List<LibraryItem> borrowedItems = new();

    public Member(string Name)
    {
        this.Name = Name;
    }

    public string BorrowItem(LibraryItem item)
    {
        if (borrowedItems.Count >= 3)
            return "You cannot borrow more than 3 items.";

        borrowedItems.Add(item);
        return $"'{item.Title}' has been added to {name}'s borrowed list.";
    }

    public string ReturnItem(LibraryItem item)
    {
        if (borrowedItems.Remove(item))
            return $"'{item.Title}' has been successfully returned.";
        return $"'{item.Title}' was not in the borrowed list.";
    }

    public List<LibraryItem> GetBorrowedItems()
    {
        return borrowedItems;
    }
}


public class LibraryManager
{
    private readonly List<LibraryItem> catalog = new();
    private readonly List<Member> members = new();

    public void AddItem(LibraryItem item)
    {
        catalog.Add(item);
    }

    public void RegisterMember(Member member)
    {
        members.Add(member);
    }

    public void ShowCatalog()
    {
        Console.WriteLine("Library Catalog :");
        foreach (var item in catalog)
            Console.WriteLine(item.GetDetails());
        Console.WriteLine("_______________________________________\n");
    }

    public LibraryItem? FindItemById(string id) => catalog.Find(i => i.Id == id);
    public Member? FindMemberByName(string name) => members.Find(m => m.Name == name);
}

class Program
{
    static void Main()
    {
        LibraryManager library = new();

        // add books
        library.AddItem(new Novels("1", "Harry Potter", "J.K. Rowling"));
        library.AddItem(new TextBook("2", "My Struggle", "Syl"));
        library.AddItem(new Magazine("3", "My Rich Roommate", 404));
        library.AddItem(new Novels("4", "Ultraman", "Eiji Tsuburaya"));

        // create members
        Member alice = new("Alice");
        Member bob = new("Bob");
        library.RegisterMember(alice);
        library.RegisterMember(bob);

        library.ShowCatalog();

        for (int i = 1; i <= 3; i++)
        {
            var item = library.FindItemById(i.ToString());
            if (item != null) Console.WriteLine(alice.BorrowItem(item));
        }

        var newNovel = new Novels("4", "Sherlock Holmes", "Conan Doyle");
        library.AddItem(newNovel);
        Console.WriteLine(alice.BorrowItem(newNovel));

    }
}