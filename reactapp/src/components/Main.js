import { useState } from 'react';
import { Container, Button, Stack, ProgressBar, Form } from 'react-bootstrap';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Header from './Header.js'
import InputForm from './InputForm.js'
import OutputArea from './OutputArea.js'

export function Main() {
    const [connection, setConnection] = useState(undefined);
    const [inProgress, setInProgress] = useState(false);
    const [encodedString, setEncodedString] = useState([]);
    const [progress, setProgress] = useState(0);

    const calculatePercent = (current, total) => current * 100 / total;

    const onNextCharacterReceived = async (character, index, total, isLast) => {
        console.log(`"New character received:${character}, ${index}/${total}`)

        setProgress(calculatePercent(index + 1, total));

        if (isLast)
        {
            console.log("All characters are received.")

            //Wait for animation to complete when progres is 100%
            setTimeout(() => setInProgress(false), 1000);
        }
    };

    const onError = (errorMessage) => {
        let message ='Server error: ' + errorMessage;
        alert(message);
        console.log(message);

        setInProgress(false);
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
           
            setProgress(0);
            setInProgress(true);
        } catch (e) {
            console.log(e);
        }
    }

    const onCancelClick = async () => {
        if (connection !== undefined)
        {
            await connection.invoke("StopEncoding");
        }

        setInProgress(false);
    }

    return (
        <>
            <Container>
                <Stack gap={4}>                      
                    <Header/>
                                                                      
                    <InputForm formId="inputForm" onValidationSucceed={onInputTextValid} disabled={inProgress}/>
                
                    <Stack direction="horizontal" gap={3}>
                        <Button disabled={inProgress} type="submit" form="inputForm">Convert</Button>   
                        <Button disabled={!inProgress} type="button" onClick={onCancelClick}>Cancel</Button>
                        <ProgressBar animated now={progress} variant='info' className='ms-auto' hidden={!inProgress} />
                    </Stack>
                
                    <OutputArea value={encodedString} enabled={inProgress}/>                
                </Stack>
            </Container>       
        </>
    );
}