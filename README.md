# GitTask
This application was a part of my Bachelor Thesis. It is a simple task management application written in C#/MVVM, aimed at programming projects which employ Git for version control.
What is special about this solution is that it stores its data in a hidden folder in the location of our project's Git repository (hence the name, GitTask).
This enables user to preview history of tasks just the same way as he can preview Git repo history.

Data is stored in JSON files, where each file is a single entity. Model is very simple, there are only a few entities: Tasks, Task States, Comments and Users.

Application provides basic functionality like creating and manipulating tasks, writting comments etc.
Interesting part is the ability to review history of project or single tasks, which is specified by searching commits from repo.

For manipulating Git repository I used libgit2sharp library. I also employed Ninject for IoC, JSON.NET for JSONs.
I used free icons from here: http://www.flaticon.com/packs/free-basic-ui-elements (created by user Lucy G).

This program is still only a draft. User interface leaves much to be desired. I also know about a few nasty bugs that can happen from time to time.
However, it was written specificly for my thesis, which was more about elaborating on my ideas than actually implementing them. 
