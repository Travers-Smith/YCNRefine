import React, { Component } from 'react';
import './custom.css';
import YCNRefine from './YCNRefine';

export default class App extends Component {
  static displayName = App.name;

  render() {
    return (
        <YCNRefine/>
    );
  }
}
