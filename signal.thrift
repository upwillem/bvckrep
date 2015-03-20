namespace csharp bcvkSignal

service Signal
{
	void CreateMainAccount(1:string username, 2:string password1, 3:string password2, 4:string email, 5:string name),
	void CreateSubAccount(1:string username, 2:string password1, 3:string password2, 4:string name, 5:binary profileImage),
	list<string> Login(1:string username, 2:string password),
	void LogOut(1:string username),
	list<string> GetAccountData(1:string username),
	i32 DoCall(1:string sender, 2:string recipient),
	void AnswerCall(1:string sender, 2:string recipient, 3:i32 callId, 4:string answer),
	string GetCallStatus(1:i32 callId),
	void EndCall(1:string sender, 2:string recipient, 3:i32 callId),
	bool ToggleBlock(1:string sender, 2:string recipient)
}