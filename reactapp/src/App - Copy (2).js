import { useState } from 'react';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import InputForm from './components/InputForm';
import OutputForm from './components/OutputForm';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';

const App = () => {
        const [connection, setConnection] = useState();
        const [encodedString, setEncodedString] = useState([""]);

        const startEncoding = async (message) => {
            try {
                const connection = new HubConnectionBuilder()
                    .withUrl("https://localhost:7014/tasks")
                    .configureLogging(LogLevel.Information)
                    .build();

                connection.on("ReceiveMessage", (symbol) => {
                    encodedString.push(symbol);
                    setEncodedString(encodedString);
                });

                connection.onclose(e => {
                    setConnection();
                });

                await connection.start();
                await connection.invoke("RequestEncoding", message);
                setConnection(connection);

                encodedString = [""];
                setEncodedString(encodedString);
            } catch (e) {
                console.log(e);
            }
        }

        const closeConnection = async () => {
            try {
                await connection.stop();
            } catch (e) {
                console.log(e);
            }
        }

    return <div className='app'>
            <h2>Encode to Base64</h2>
            <hr className='line' />
            <InputForm startEncoding={startEncoding} />
            <OutputForm closeConnection={closeConnection} messages= {encodedString} />
    </div>
}

export default App;