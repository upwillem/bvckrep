namespace csharp bcvkStream

service Stream
{
	void SendStream(1:string sender, 2:string recipient, 3:list<binary> stream, 4:string connectId, 5:bool audio),
	void SendVideo(1:string sender, 2:string recipient, 3:list<binary> video, 4:string connectId, 5:bool audio),
	list<binary> GetStream(1:string sender, 2:string recipient, 3:string connectId, 4:bool audio),
	list<binary> GetVideo(1:string sender, 2:string recipient, 3:string connectId, 4:bool audio)
}