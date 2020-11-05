import { Component, OnInit, ViewChild } from '@angular/core';

@Component({
    selector: 'app-video-call',
    templateUrl: './video-call.component.html',
    styleUrls: ['./video-call.component.scss']
})
export class VideoCallComponent implements OnInit {
  @ViewChild('videoElement', {static: true}) videoElement: any;  
  video: any;

  public mute: boolean;

  ngOnInit() {
    this.video = this.videoElement.nativeElement;
    this.initCamera({ video: true, audio: this.mute });
  }

  public toggleMute() {
    this.mute = !this.mute;
    this.initCamera({ video: true, audio: this.mute });
  }

  public close() {
    this.initCamera({ video: false, audio: false });
  }

  private initCamera(config: any) {
    const browser = <any>navigator;

    browser.getUserMedia = (browser.getUserMedia ||
      browser.webkitGetUserMedia ||
      browser.mozGetUserMedia ||
      browser.msGetUserMedia);

    browser.mediaDevices.getUserMedia(config).then(stream => {
      this.video.src = window.URL.createObjectURL(stream);
      this.video.play();
    });
  }
}
