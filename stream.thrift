namespace csharp bcvkStream

service Stream
{
	void SendStream(1:string sender, 2:string recipient, 3:list<binary> stream, 4:string connectId),
	void SendVideo(1:string sender, 2:string recipient, 3:list<binary> video, 4:string connectId),
	binary GetStream(1:string sender, 2:string recipient, 3:string connectId),
	binary GetVideo(1:string sender, 2:string recipient, 3:string connectId)
}