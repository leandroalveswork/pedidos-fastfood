import React from 'react';
import css from '../css/Header.module.css';

export default function Header() {
    return (
        <div className={css.HeaderContainer}>
            <header>_</header>
            <header className={css.show}><span><em>Burger Prince</em></span></header>
        </div>
    )
}
