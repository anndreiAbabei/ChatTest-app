/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { VideoCallComponent } from './video-call.component';

let component: VideoCallComponent;
let fixture: ComponentFixture<VideoCallComponent>;

describe('video-call component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ VideoCallComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(VideoCallComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});