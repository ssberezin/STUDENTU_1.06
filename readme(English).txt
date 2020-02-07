
Programming - because I like it!)

My English level is A1, so I apologize in advance for a possible incorrect translation from google translater

This project is not finished and is written for your personal business (see studhelp.pro)
The project is being implemented in conjunction with WPF + ADO.NET technologies, in C #.


Project goal: a workable application for receiving orders in the provision of services.
The main direction: the implementation of student work.
The project is implemented as part of a non-strict MVVM pattern. Closing the window runs counter to this feature, but ... I don’t want to redo this moment yet.

When implementing the project, the "Code First" approach was used.
If you want to look at the result of work, you need to enter the name of your sql server in the connection string to the database in the "App.config" file.
The commit from 7.2.20 contains a project that already knows how to:
- make an entry in the database about the accepted order;
- carry out to a certain extent the editing of these same "orders";
- to edit data on authors (performers).

Not implemented at time 7.2.20:
- authorization of the software user;
- branch of order filters;
- branch of accounting for expenses and income;
- branch printout receipts;
- a branch of the black list (both for customers and for authors).

Well, along the way, maybe something else will come up.

This is if in short and in general.