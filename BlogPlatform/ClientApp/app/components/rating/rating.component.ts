import { Component, OnInit, Input, Output, EventEmitter, HostListener, forwardRef, NgModule } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { Constants } from '../../core/constants';

@Component({
    selector: 'rating',
    template: require('./rating.component.html'),
    styles: [require('./rating.component.css')],
    providers: [
        { provide: NG_VALUE_ACCESSOR, useExisting: forwardRef(() => RatingComponent), multi: true }
    ]
})

export class RatingComponent implements OnInit, ControlValueAccessor {
    @Input()
    iconClass = 'star-icon';

    @Input()
    fullIcon = '★';

    @Input()
    emptyIcon = '☆';

    @Input()
    float: boolean;

    @Input()
    titles: string[] = [];

    @Input()
    readonly: boolean;

    @Input()
    disabled: boolean;

    @Input()
    set max(max: number) {
        this.maxValue = max;
        this.buildRanges();
    }

    get max() {
        return this.maxValue;
    }

    @Output()
    onHover = new EventEmitter();

    @Output()
    onLeave = new EventEmitter();

    public model: number;
    public ratingRange: number[];
    public hovered: number = 0;
    public hoveredPercent: number = undefined;

    private maxValue: number = Constants.RatingMaxValue;
    private onChange: (m: any) => void;
    private onTouched: (m: any) => void;
    
    ngOnInit() {
        this.buildRanges();
    }

    writeValue(value: number): void {
        this.model = value;
    }

    registerOnChange(functionReference: any): void {
        this.onChange = functionReference;
    }

    registerOnTouched(functionReference: any): void {
        this.onTouched = functionReference;
    }

    public calculateWidth(item: number): number {
        if (this.hovered > 0) {
            if (this.hoveredPercent !== undefined && this.hovered === item)
                return this.hoveredPercent;

            return this.hovered >= item ? 100 : 0;
        }
        return this.model >= item ? 100 : 100 - Math.round((item - this.model) * 10) * 10;
    }

    public setHovered(hovered: number): void {
        if (!this.readonly && !this.disabled) {
            this.hovered = hovered;
            this.onHover.emit(hovered);
        }
    }

    public changeHovered(event: MouseEvent): void {
        if (!this.float) return;
        const target = event.target as HTMLElement;
        const relativeX = event.pageX - target.offsetLeft;
        const percent = Math.round((relativeX * 100 / target.offsetWidth) / 10) * 10;
        this.hoveredPercent = percent > 50 ? 100 : 50;
    }

    public resetHovered(): void {
        this.hovered = 0;
        this.hoveredPercent = undefined;
        this.onLeave.emit(this.hovered);
    }

    public rate(value: number) {
        if (!this.readonly
            && !this.disabled
            && value >= 0
            && value <= this.ratingRange.length) {
            const newValue = this.hoveredPercent ? (value - 1) + this.hoveredPercent / 100 : value;
            this.onChange(newValue);
            this.model = newValue;
        }
    }

    private buildRanges() {
        this.ratingRange = [];
        for (let i = 1; i <= this.max; i++) {
            this.ratingRange.push(i);
        }
    }
}