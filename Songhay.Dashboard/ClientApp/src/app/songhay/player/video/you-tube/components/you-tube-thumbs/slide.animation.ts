import {
    AnimationReferenceMetadata,
    animate,
    animation,
    style
} from '@angular/animations';

export const slideAnimation = {
    id: 'slide-right',
    animation: animation(
        [
            style({ left: '{{ x1 }}px' }),
            animate('{{ time }} ease-in', style({ left: '{{ x2 }}px' }))
        ],
        { params: { time: '700ms', x1: 0, x2: 100 } }
    )
};

export const slideAnimations = new Map<string, AnimationReferenceMetadata>([
    [slideAnimation.id, slideAnimation.animation]
]);
