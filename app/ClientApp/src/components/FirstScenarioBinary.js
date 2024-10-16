import React, {Component} from "react";

export class FirstScenarioBinary extends Component {
    static displayName = FirstScenarioBinary.name;
    
    render() {
        return (
            <div>
                <h1>First scenario - <strong>Binary Vector</strong> </h1>
                <p>In this scenario the user is able to write a binary vector according to the size of a provided
                    generator matrix.</p>
                <p>After it is written, the binary vector is encoded using <strong>the Linear Encoding algorithm</strong>,
                    then sent through a <strong>Channel </strong> where errors in the vector can be introduced. </p>
                <p>After that, the vector is decoded and the message is displayed. Any discrepancies from the original 
                    message are highlighted.</p>
            </div>
        );
    }
}