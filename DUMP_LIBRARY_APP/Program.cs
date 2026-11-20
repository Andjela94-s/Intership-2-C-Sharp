using System.Net.NetworkInformation;
using System.Xml.Serialization;

const int LoanDays = 21;
const decimal LateFeePerDay = 0.5m;

var Members = new Dictionary<int, (string FirstName, string LastName, DateTime DateOfBirth,
    List<(int BookId, string Title, string Author, DateTime BorrowedDate, DateTime? DueDate)> Loans)>
{
    [1] = ("Ana", "Anic", new DateTime(1990, 1, 1), new List<(int, string, string, DateTime, DateTime?)> { (1, "Na Drini cuprija", "Ivo Andrić", new DateTime(2026, 9, 1), new DateTime(2026, 10, 21)), (2, "Zlatarovo zlato", "August Šenoa", new DateTime(2026, 1, 10), new DateTime(2026, 1, 28)) }),
    [2] = ("Ivan", "Ivic", new DateTime(1985, 5, 15), new List<(int, string, string, DateTime, DateTime?)> { (3, "Ljetni vjetar", "Miroslav Krleža", new DateTime(2023, 1, 1), new DateTime(2023, 1, 21)), (4, "Harry Potter i Kamen mudraca", "J. K. Rowling", new DateTime(2026, 10, 20), null) }),
    [3] = ("Marko", "Markovic", new DateTime(1995, 10, 20), new List<(int, string, string, DateTime, DateTime?)> { (3, "Trilogija", "Milorad Pavić", new DateTime(2024, 11, 11), new DateTime(2024, 11, 21)), (6, "Zločin i kazna", "Fjodor Dostojevski", new DateTime(2026, 11, 1), null) }),
};

Main();
void Main()
{
    while (true)
    {
        Console.WriteLine("Library Management System\n");
        Console.WriteLine("1-Members\t2-Borrowed books\t3-Statistics\t0-Exit the app");
        var menu = Console.ReadLine();
        switch (menu)
        {
            case "1":
                Console.WriteLine("Loading members...");
                LibraryMembers();
                break;
            case "2":
                Console.WriteLine("Loading borrowed books...");
                BorrowedBooks();
                break;
            case "3":
                Console.WriteLine("Loading statistics...");
                Statistics();
                break;
            case "0":
                Console.WriteLine("Leaving the app..");
                return;
            default:
                Console.WriteLine("Wrong input!");
                break;
        }
    }
}
void LibraryMembers()
{

    Console.WriteLine("1-Add new member\n2-Delete member\n3-Edit member\n4-See all members\n0-return to main menu");
    var members = Console.ReadLine();
    switch (members)
    {
        case "1":
            Console.WriteLine("Adding a new member...");
            AddMembers();
            break;
        case "2":
            Console.WriteLine("Deleting a member...");
            DeleteMembers();
            break;
        case "3":
            Console.WriteLine("Editing a member...");
            EditMembers();
            break;
        case "4":
            Console.WriteLine("View of all members...");
            ViewAllMembers();
            break;
        case "0":
            Console.WriteLine("Return to main menu...");
            Main();
            return;
        default:
            Console.WriteLine("Wrongly selected number!");
            break;
    }
}

void AddMembers()
{
    Console.WriteLine("Enter first name:");
    var firstName = Console.ReadLine();
    Console.WriteLine("Enter last name:");
    var lastName = Console.ReadLine();
    Console.WriteLine("Enter date of birth (YYYY-MM-DD):");
    DateTime dateOfBirth = DateTime.Parse(Console.ReadLine());
    if (dateOfBirth > DateTime.Now)
    {
        Console.WriteLine("Date of birth cannot be in the future.");
        return;
    }
    Members.Add(Members.Count + 1, (firstName, lastName, dateOfBirth, new List<(int, string, string, DateTime, DateTime?)>()));
    Console.WriteLine($"Member {firstName} {lastName} added successfully.");
}
void DeleteMembers()
{
    Console.WriteLine("Are you sure you want to delete this member? Y/N");
    char choice = Console.ReadLine()[0];
    if (choice == 'Y' || choice == 'y')
    {
        Console.WriteLine("Enter member ID to delete:");
        var memberIdInput = Console.ReadLine();
        if (int.TryParse(memberIdInput, out int memberId) && Members.ContainsKey(memberId))
        {
            Members.Remove(memberId);
            Console.WriteLine($"Member with ID {memberId} deleted successfully.");
        }
        else
        {
            Console.WriteLine("Invalid member ID.");
        }
    }
    else if(choice == 'N' || choice == 'n')
    {
        Console.WriteLine("Returning to previous menu...");
        return;
    }
    else
    {
        Console.WriteLine("Wrong input!");
        return;
    }
}
void EditMembers()
{
    Console.WriteLine("Are you sure you want to edit a member? Y/N");
    char choice = Console.ReadLine()[0];
    if (choice == 'Y' || choice == 'y')
    {
        Console.WriteLine("Enter member ID to edit:");
        var memberIdInput = Console.ReadLine();
        if (int.TryParse(memberIdInput, out int memberId) && Members.ContainsKey(memberId))
        {
            var member = Members[memberId];
            Console.WriteLine($"Editing member: {member.FirstName} {member.LastName}");
            Console.WriteLine("Enter new first name (leave blank to keep current):");
            var newFirstName = Console.ReadLine();
            Console.WriteLine("Enter new last name (leave blank to keep current):");
            var newLastName = Console.ReadLine();
            Console.WriteLine("Enter new date of birth (YYYY-MM-DD) (leave blank to keep current):");
            var newDateOfBirthInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newFirstName)) member.FirstName = newFirstName;
            if (!string.IsNullOrWhiteSpace(newLastName)) member.LastName = newLastName;
            if (!string.IsNullOrWhiteSpace(newDateOfBirthInput)) member.DateOfBirth = DateTime.Parse(newDateOfBirthInput);
            Members[memberId] = member;
            Console.WriteLine($"Member with ID {memberId} updated successfully.");
        }
        else
        {
            Console.WriteLine("Invalid member ID.");
        }
    }
    else if(choice == 'N' || choice == 'n')
    {
        Console.WriteLine("Returning to menu...");
        return;
    }
    else
    {
        Console.WriteLine("Wrong input!");
        return;
    }
}
void ViewAllMembers()
{
    foreach (var kvp in Members)
    {
        var member = kvp.Value;
        Console.WriteLine($"ID: {kvp.Key}, Name: {member.LastName} {member.FirstName}, DOB: {member.DateOfBirth.ToShortDateString()}");

    }
}
void BorrowedBooks()
{
    while (true)
    {
        Console.WriteLine("1-Borrow a book\n2-Return a book\n3-Delete a borrow\n4-Edit a borrow\n5-View borrowed books\n6-Data and analytics\n0-return to main menu");
        var borrowedBooks = Console.ReadLine();
        switch (borrowedBooks)
        {
            case "1":
                Console.WriteLine("Borrowing a book...");
                BorrowBook();
                break;
            case "2":
                Console.WriteLine("Returning a book...");
                ReturnBook();
                break;
            case "3":
                Console.WriteLine("Deleting a borrow...");
                DeleteBorrow();
                break;
            case "4":
                Console.WriteLine("Editing a borrow...");
                EditBorrow();
                break;
            case "5":
                Console.WriteLine("Viewing borrowed books...");
                ViewBorrowedBooks();
                break;
            case "6":
                Console.WriteLine("Viewing data and analytics...");
                ViewDataAndAnalytics();
                break;
            case "0":
                Console.WriteLine("Return to main menu...");
                Main();
                return;
            default:
                Console.WriteLine("Wrongly selected number!");
                break;
        }
    }
}
void BorrowBook()
{
    Console.WriteLine("Enter member ID:");
    var memberIdInput = Console.ReadLine();
    if (int.TryParse(memberIdInput, out int memberId) && Members.ContainsKey(memberId))
    {
        var member = Members[memberId];
        Console.WriteLine("Enter book ID:");
        var bookIdInput = Console.ReadLine();
        if (int.TryParse(bookIdInput, out int bookId))
        {
            Console.WriteLine("Enter book title:");
            var title = Console.ReadLine();
            Console.WriteLine("Enter book author:");
            var author = Console.ReadLine();
            var borrowedDate = DateTime.Now;
            var dueDate = borrowedDate.AddDays(LoanDays);
            member.Loans.Add((bookId, title, author, borrowedDate, dueDate));
            Members[memberId] = member;
            Console.WriteLine($"Book '{title}' borrowed successfully. Due date: {dueDate.ToShortDateString()}");
        }
        else
        {
            Console.WriteLine("Invalid book ID.");
        }
    }
    else
    {
        Console.WriteLine("Invalid member ID.");
    }
}
void ReturnBook()
{
    Console.WriteLine("Enter member ID:");
    var memberIdInput = Console.ReadLine();
    if (int.TryParse(memberIdInput, out int memberId) && Members.ContainsKey(memberId))
    {
        var member = Members[memberId];
        Console.WriteLine("Enter book ID to return:");
        var bookIdInput = Console.ReadLine();
        if (int.TryParse(bookIdInput, out int bookId))
        {
            var loan = member.Loans.FirstOrDefault(l => l.BookId == bookId);
            if (loan != default)
            {
                member.Loans.Remove(loan);
                Members[memberId] = member;
                var lateDays = (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days;
                if (lateDays > 0)
                {
                    var lateFee = lateDays * LateFeePerDay;
                    Console.WriteLine($"Book '{loan.Title}' returned successfully. Late fee: ${lateFee}");
                }
                else
                {
                    Console.WriteLine($"Book '{loan.Title}' returned successfully.");
                }
            }
            else
            {
                Console.WriteLine("This book was not borrowed by the member.");
            }
        }
        else
        {
            Console.WriteLine("Invalid book ID.");
        }
    }
    else
    {
        Console.WriteLine("Invalid member ID.");
    }
}
void DeleteBorrow()
{
    Console.WriteLine("Are you sure you want to delete this loan? Y/N");
    char choice = Console.ReadLine()[0];
    if (choice == 'Y' || choice == 'y')
    {
        Console.WriteLine("Enter member ID:");
        var memberIdInput = Console.ReadLine();
        if (int.TryParse(memberIdInput, out int memberId) && Members.ContainsKey(memberId))
        {
            var member = Members[memberId];
            Console.WriteLine("Enter book ID to delete borrow:");
            var bookIdInput = Console.ReadLine();
            if (int.TryParse(bookIdInput, out int bookId))
            {
                var loan = member.Loans.FirstOrDefault(l => l.BookId == bookId);
                if (loan != default)
                {
                    member.Loans.Remove(loan);
                    Members[memberId] = member;
                    Console.WriteLine($"Borrow record for book '{loan.Title}' deleted successfully.");
                }
                else
                {
                    Console.WriteLine("This book was not borrowed by the member.");
                }
            }
            else
            {
                Console.WriteLine("Invalid book ID.");
            }
        }
        else
        {
            Console.WriteLine("Invalid member ID.");
        }
    }
    else if(choice == 'N' || choice == 'n')
    {
        Console.WriteLine("Returning to menu...");
        return;
    }
    else
    {
        Console.WriteLine("Wrong input!");
        return;
    }
}
void EditBorrow()
{
    Console.WriteLine("Are you sure you want to edit a book? Y/N");
    char choice = Console.ReadLine()[0];
    if (choice == 'Y' || choice == 'y')
    {
        Console.WriteLine("Enter member ID:");
        var memberIdInput = Console.ReadLine();
        Console.WriteLine("Enter book ID to edit:\n1-Title\n2-Author\n3-Borrowed Date\n4-Due Date");
        var bookIdInput = Console.ReadLine();
        var fieldToEdit = Console.ReadLine();
        switch (fieldToEdit)
        {
            case "1":
                Console.WriteLine("Enter new title:");
                var newTitle = Console.ReadLine();
                bookIdInput = newTitle;
                break;
            case "2":
                Console.WriteLine("Enter new author:");
                var newAuthor = Console.ReadLine();
                bookIdInput = newAuthor;
                break;
            case "3":
                Console.WriteLine("Enter new borrowed date (YYYY-MM-DD):");
                var newBorrowedDateInput = Console.ReadLine();
                DateTime newBorrowedDate = DateTime.Parse(newBorrowedDateInput);
                break;
            case "4":
                Console.WriteLine("Enter new due date (YYYY-MM-DD):");
                var newDueDateInput = Console.ReadLine();
                DateTime newDueDate = DateTime.Parse(newDueDateInput);
                break;
            default:
                Console.WriteLine("Invalid selection.");
                break;
        }
    }
    else if(choice == 'N' || choice == 'n')
    {
        Console.WriteLine("Returning to menu...");
        return;
    }
    else
    {
        Console.WriteLine("Wrong input!");
        return;
    }
}
void ViewBorrowedBooks()
{
    while(true)
    {
    Console.WriteLine("1-All borrowed books\n2-Sorted books by loan date\n3-Sorted books by late days\n4-Currently borrowed books\n" +
        "5-Search by title or author\n6-Search by month");
    var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("Viewing all borrowed books...");
                ViewAllBorrowedBooks();
                break;
            case "2":
                Console.WriteLine("Viewing sorted books by loan date...");
                ViewSortedByLoanDate();
                break;
            case "3":
                Console.WriteLine("Viewing sorted books by late days...");
                ViewSortedByLateDays();
                break;
            case "4":
                Console.WriteLine("Viewing currently borrowed books...");
                ViewCurrentlyBorrowedBooks();
                break;
            case "5":
                Console.WriteLine("Searching by title or author...");
                SearchByTitleOrAuthor();
                break;
            case "6":
                Console.WriteLine("Searching by month...");
                SearchByMonth();
                break;
                case "0":
                Console.WriteLine("Return to main menu...");
                Main();
                return;
            default:
                Console.WriteLine("Wrongly selected number!");
                break;
        }
    }
}
void ViewDataAndAnalytics()
{
    Console.WriteLine("Enter member ID:");
    var memberIdInput = Console.ReadLine();
    if (int.TryParse(memberIdInput, out int memberId) && Members.ContainsKey(memberId))
    {
        var member = Members[memberId];
        Console.WriteLine($"Data and Analytics for {member.FirstName} {member.LastName}:");
        Console.WriteLine($"Total Books Borrowed: {member.Loans.Count} and currently borrowed: {member.Loans.Count(l => l.DueDate.HasValue && l.DueDate.Value > DateTime.Now)}");
        Console.WriteLine($"Total Late Fees: ${member.Loans.Sum(l => (DateTime.Now - l.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - l.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0)}");
        Console.WriteLine($"Average Borrowed Days: {member.Loans.Average(l => (DateTime.Now - l.BorrowedDate).Days):F2}");
        Console.WriteLine($"Longest Late Borrowed Days: {member.Loans.Max(l => (DateTime.Now - l.BorrowedDate).Days)}");
        Console.WriteLine($"Borrowed books from date {member.Loans.Min(l => l.BorrowedDate).ToShortDateString()} to {member.Loans.Max(l => l.BorrowedDate).ToShortDateString()}");
    }
    else
    {
        Console.WriteLine("Invalid member ID.");
    }
}
void ViewAllBorrowedBooks()
{
    foreach(var kvp in Members)
    {
        var member = kvp.Value;
        foreach (var loan in member.Loans)
        {
            Console.WriteLine($"{loan.BookId} \"{loan.Title}\" - {loan.Author} | Borrowed Date: {loan.BorrowedDate.ToShortDateString()}" +
                $"| Returned Date: {(loan.DueDate.HasValue ? loan.DueDate.Value.ToShortDateString() : "Not Returned")}" +
                $"| Total Late Days: {((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days : 0)}" +
                $"| Late fee: ${((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0):F2}");
        }
    }
}
void ViewSortedByLoanDate()
{
    Console.WriteLine("Type 1 for ascending order, 2 for descending order:");
    var choice = Console.ReadLine();
    var sortedLoans = choice == "1" ? Members.Values.SelectMany(m => m.Loans).OrderBy(l => l.BorrowedDate)
                                     : Members.Values.SelectMany(m => m.Loans).OrderByDescending(l => l.BorrowedDate);

    foreach (var loan in sortedLoans)
    {
        Console.WriteLine($"{loan.BookId} \"{loan.Title}\" - {loan.Author} | Borrowed Date: {loan.BorrowedDate.ToShortDateString()}" +
            $"| Returned Date: {(loan.DueDate.HasValue ? loan.DueDate.Value.ToShortDateString() : "Not Returned")}" +
            $"| Total Late Days: {((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days : 0)}" +
            $"| Late fee: ${((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0):F2}");
    }
}
void ViewSortedByLateDays()
{
    Console.WriteLine("Type 1 for ascending order, 2 for descending order:");
    var choice = Console.ReadLine();
    var sortedLoans = choice == "1" ? Members.Values.SelectMany(m => m.Loans).OrderBy(l => (DateTime.Now - l.DueDate.GetValueOrDefault()).Days)
                                     : Members.Values.SelectMany(m => m.Loans).OrderByDescending(l => (DateTime.Now - l.DueDate.GetValueOrDefault()).Days);
    foreach (var loan in sortedLoans)
    {
        Console.WriteLine($"{loan.BookId} \"{loan.Title}\" - {loan.Author} | Borrowed Date: {loan.BorrowedDate.ToShortDateString()}" +
            $"| Returned Date: {(loan.DueDate.HasValue ? loan.DueDate.Value.ToShortDateString() : "Not Returned")}" +
            $"| Total Late Days: {((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days : 0)}" +
            $"| Late fee: ${((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0):F2}");
    }
}
void ViewCurrentlyBorrowedBooks()
{
    var currentlyBorrowed = Members.Values.SelectMany(m => m.Loans).Where(l => !l.DueDate.HasValue || l.DueDate.Value > DateTime.Now);
    foreach (var loan in currentlyBorrowed)
    {
        Console.WriteLine($"{loan.BookId} \"{loan.Title}\" - {loan.Author} | Borrowed Date: {loan.BorrowedDate.ToShortDateString()}" +
            $"| Due Date: {(loan.DueDate.HasValue ? loan.DueDate.Value.ToShortDateString() : "Not Set")}" +
            $"| Total Late Days: {((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days : 0)}" +
            $"| Late fee: ${((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0):F2}");
    }
}
void SearchByTitleOrAuthor()
{
    Console.WriteLine("Enter title or author to search:");
    var searchTerm = Console.ReadLine().ToLower();
    var results = Members.Values.SelectMany(m => m.Loans)
        .Where(l => l.Title.ToLower().Contains(searchTerm) || l.Author.ToLower().Contains(searchTerm));
    foreach (var loan in results)
    {
        Console.WriteLine($"{loan.BookId} \"{loan.Title}\" - {loan.Author} | Borrowed Date: {loan.BorrowedDate.ToShortDateString()}" +
            $"| Returned Date: {(loan.DueDate.HasValue ? loan.DueDate.Value.ToShortDateString() : "Not Returned")}" +
            $"| Total Late Days: {((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days : 0)}" +
            $"| Late fee: ${((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0):F2}");
    }
}
void SearchByMonth()
{
    Console.WriteLine("Enter month (1-12) to search:");
    var monthInput = Console.ReadLine();
    if (int.TryParse(monthInput, out int month) && month >= 1 && month <= 12)
    {
        var results = Members.Values.SelectMany(m => m.Loans)
            .Where(l => l.BorrowedDate.Month == month);
        foreach (var loan in results)
        {
            Console.WriteLine($"{loan.BookId} \"{loan.Title}\" - {loan.Author} | Borrowed Date: {loan.BorrowedDate.ToShortDateString()}" +
                $"| Returned Date: {(loan.DueDate.HasValue ? loan.DueDate.Value.ToShortDateString() : "Not Returned")}" +
                $"| Total Late Days: {((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days : 0)}" +
                $"| Late fee: ${((DateTime.Now - loan.DueDate.GetValueOrDefault()).Days > 0 ? (DateTime.Now - loan.DueDate.GetValueOrDefault()).Days * LateFeePerDay : 0):F2}");
        }
    }
    else
    {
        Console.WriteLine("Invalid month.");
    }
}
void Statistics()
{
    while (true)
    {
        Console.WriteLine("1-Member with the biggest late fee\n2-Member with the most loans\n3-The most loaned title\n" +
            "4-Sum of all late fees\n5-Graph of monthly loans\n0-Return to main menu");
        var choice = Console.ReadLine();
        switch (choice)
        {
            case "1":
                Console.WriteLine("Loading the member with the biggest late fee...");
                MemberWithBiggestFee();
                break;
            case "2":
                Console.WriteLine("Loading the member with the most loans...");
                MemberWithTheMostLoans();
                break;
            case "3":
                Console.WriteLine("Loading the most loaned title...");
                MostLoanedTitle();
                break;
            case "4":
                Console.WriteLine("Loading the sum of all members fees...");
                SumOfAllLateFees();
                break;
            case "5":
                Console.WriteLine("Loading the monthly loans graph...");
                MonthlyLoansGraph();
                break;
            case "0":
                Console.WriteLine("Returning to menu...");
                return;
            default:
                Console.WriteLine("Wrong input!");
                break;
        }
    }
}
void MemberWithBiggestFee()
{

}
void MemberWithTheMostLoans()
{

}
void MostLoanedTitle()
{

}
void SumOfAllLateFees()
{

}
void MonthlyLoansGraph()
{

}
