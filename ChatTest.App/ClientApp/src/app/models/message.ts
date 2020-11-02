interface Message {
  
  id: string;
  conversationId: string;
  sender: string;
  createdAt: Date;
  text: string;
  isMine: boolean;
}
