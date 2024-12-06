import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render() {
    return (
      <div>
        <h1>Coding Theory Project</h1>
          <p>This project is designed to showcase the linear encoding and step-by-step decoding algorithms.</p>
            <p>To achieve this, three scenarios are present to showcase the encoding/decoding features, ranked by difficulty. </p>
          <b></b>
            <p>Binary vector coding represents the simpliest demonstration of both algorithms, text coding scenario merges many
            binary vector codings into one, where as image coding is the final and most programmically challenging part of this project.</p>
      </div>
    );
  }
}
