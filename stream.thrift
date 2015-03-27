namespace csharp bcvkStream

service Stream
{
	void SendStream(1:string sender, 2:string recipient, 3:list<binary> stream),
	void SendVideo(1:string sender, 2:string recipient, 3:list<binary> video),
	binary GetStream(1:string sender, 2:string recipient),
	binary GetVideo(1:string sender, 2:string recipient)
}