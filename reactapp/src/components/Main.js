import { useState } from 'react';
import { Container, Button, Stack } from 'react-bootstrap';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Header from './Header.js'
import InputForm from './InputForm.js'
import OutputArea from './OutputArea.js'

export function Main() {
    const [connection, setConnection] = useState(undefined);
    const [isEncodingInProgress, setIsEncodingInProgress] = useState(false);
    const [encodedString, setEncodedString] = useState([]);

    const onNextCharacterReceived = (character, index, total, isLast) => {
        console.log(`"New character received:${character}, ${index}/${total}`)

        if (isLast)
        {
            console.log("All characters are received.")

            setIsEncodingInProgress(false);
        }
    };

    const onError = (errorMessage) => {
        let message ='Server error: ' + errorMessage;
        alert(message);
        console.log(message);

        setIsEncodingInProgress(false);
    };

    const onClose = (error) => {
        setConnection();
    };

    const onInputTextValid = async (inputText) => {
        try {
            console.log(connection);

            if (connection === undefined)
            {
                console.log("Creating a new WebSocket connection...")

                const newConnection = new HubConnectionBuilder()
                    .withUrl("https://localhost:7014/tasks")
                    .configureLogging(LogLevel.Information)
                    .build();

                newConnection.on("OnNextCharacterReceived", onNextCharacterReceived);
                newConnection.on("OnError", onError);
                newConnection.onclose(onClose);

                setConnection(newConnection);

                await newConnection.start();  
                await newConnection.invoke("StartEncoding", inputText);
                     
            } else {
                console.log("Reusing an existing WebSocket connection...")
                await connection.invoke("StartEncoding", inputText);            
            }        
           
            setIsEncodingInProgress(true);
        } catch (e) {
            console.log(e);
        }
    }

    const onCancelClick = async () => {
        if (connection !== undefined)
        {
            await connection.invoke("StopEncoding");
        }

        setIsEncodingInProgress(false);
    }

    return (
        <>
            <Container>
                <Stack gap={4}>                      
                    <Header/>
                                                                      
                    <InputForm formId="inputForm" onValidationSucceed={onInputTextValid} disabled={isEncodingInProgress}/>
                
                    <Stack direction="horizontal" gap={3}>
                        <Button disabled={isEncodingInProgress} type="submit" form="inputForm">Convert</Button>   
                        <Button disabled={!isEncodingInProgress} type="button" onClick={onCancelClick}>Cancel</Button>   
                    </Stack>            
                
                    <OutputArea value={encodedString} enabled={isEncodingInProgress}/>                
                </Stack>
            </Container>       
        </>
    );
}