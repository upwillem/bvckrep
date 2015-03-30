namespace csharp bcvkSignal

service Signal
{
	list<string> CreateMainAccount(1:string username, 2:string password1, 3:string password2, 4:string email, 5:string name),
	list<string> CreateSubAccount(1:i32 parentId, 2:string username, 3:string password1, 4:string password2, 5:string name, 6:binary profileImage),
	list<string> Login(1:string username, 2:string password),
	void LogOut(1:string username),
	list<string> GetAccountData(1:string username),
	string DoCall(1:string sender, 2:string recipient),
	void AnswerCall(1:string sender, 2:string recipient, 3:string connectionId, 4:string answer),
	string GetCallStatus(1:string connectionId),
	string GetParticipantCallStatus(1:string connectionId, 2:string participants),
	void EndCall(1:string sender, 2:string recipient, 3:string connectionId),
	bool ToggleBlock(1:string sender, 2:string recipient)
}