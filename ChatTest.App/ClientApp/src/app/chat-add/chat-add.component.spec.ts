﻿/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ChatAddComponent } from './chat-add.component';

let component: ChatAddComponent;
let fixture: ComponentFixture<ChatAddComponent>;

describe('chat-add component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ChatAddComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ChatAddComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});