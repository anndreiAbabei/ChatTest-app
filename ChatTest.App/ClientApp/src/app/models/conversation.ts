interface Conversation {
  id: string;
  name: string;
  text: string;
  read: boolean;
  online: boolean;
  participants: string[];
  createdBy: string;
  reads: { [key: string]: string};
  messages?: Message[];
}
