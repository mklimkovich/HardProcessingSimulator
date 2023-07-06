import { useState, useRef } from 'react';
import { Container, Button, Stack, ProgressBar } from 'react-bootstrap';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import Header from './Header.js'
import InputForm from './InputForm.js'
import OutputArea from './OutputArea.js'

export function Main({ serverUrl }) {
    let connection = useRef(undefined);
    let encodedString = useRef("");

    const [inProgress, setInProgress] = useState(false);    
    const [percentage, setPercentage] = useState(0);

    const calculatePercentage = (current, total) => current * 100 / total;

    const onNextCharacterReceived = async (character, index, total, isLast) => {
        encodedString.current = encodedString.current + character;
        setPercentage(calculatePercentage(index + 1, total));

        if (isLast)
        {
            //Wait for animation to complete when progres is 100%
            setTimeout(() => setInProgress(false), 1000);
        }
    };

    const onError = (errorMessage) => {
        let message = `"Error: ${errorMessage}`;

        alert(message);
        console.log(message);

        setInProgress(false);
    };

    const onClose = (error) => {
        connection.current = undefined;
    };

    const onInputTextValid = async (inputText) => {
        try {
            if (connection.current === undefined)
            {
                console.log("Creating a new WebSocket connection...")

                const newConnection = new HubConnectionBuilder()
                    .withUrl(serverUrl)
                    .configureLogging(LogLevel.Information)
                    .build();

                newConnection.on("OnNextCharacterReceived", onNextCharacterReceived);
                newConnection.on("OnError", onError);
                newConnection.onclose(onClose);

                await newConnection.start();  

                connection.current = newConnection;              
            } else {
                console.log("Reusing an existing WebSocket connection...")     
            }        
           
            await connection.current.invoke("StartEncoding", inputText);

            encodedString.current = "";
            setPercentage(0);
            setInProgress(true);
        } catch (e) {
            console.log(e);
        }
    }

    const onCancelClick = async () => {
        await connection.current?.invoke("StopEncoding");
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
                        <ProgressBar animated now={percentage} variant='info' className='ms-auto' hidden={!inProgress} />
                    </Stack>
                
                    <OutputArea value={encodedString.current}/>                
                </Stack>
            </Container>       
        </>
    );
}