import { Component, OnInit } from '@angular/core';
import { NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import { UserService } from '../services/user.service';
import { ConversationService } from '../services/conversation.service';

@Component({
    selector: 'app-chat-add',
    templateUrl: './chat-add.component.html',
    styleUrls: ['./chat-add.component.scss']
})
/** chat-add component*/
export class ChatAddComponent implements OnInit {

  conversationName: string;
  searchMembers: string;
  members: MemberModel[];
  error: string;
  
  constructor(public activeModal: NgbActiveModal,
              private readonly userSerivce: UserService,
              private readonly conversationService: ConversationService) {
  }

  ngOnInit(): void {
    this.userSerivce.getAll()
      .subscribe(
        users => this.members = users.map(u => <MemberModel>{ name: u.name,
          online: u.online,
          selected: false }),
        error => this.error = error.message
      );
  }

  public haveSelectedMembers(): boolean {
    return this.members && this.selectedMembers().length > 0;
  }

  public selectedMembers(): MemberModel[] {
    return this.members.filter(m => m.selected);
  }

  public getMemberInitials(member: Member): string {
    const def = '-';

    if (!member || !member.name)
      return def;

    const names = member.name.trim().split(' ').filter(s => s.trim().length > 0);

    if (names.length <= 0)
      return def;

    if (names.length === 1)
      return names[0].trim()[0].toUpperCase();

    return (names[0].trim()[0] + names[1].trim()[0]).toUpperCase();
  }

  public save() {
    this.error = '';

    const selectedUsers = this.selectedMembers();

    if (selectedUsers.length <= 0) {
      alert('Select the user(s) that you want to start a convesation to');
      return;
    }

    this.conversationService.create(this.conversationName, selectedUsers.map(m => m.name))
      .subscribe(r => this.activeModal.close(r),
                 error => this.error = error.error || error.message);
  }
}
