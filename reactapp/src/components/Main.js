import { useState } from 'react';
import { Container, Form, Row, Button, Stack } from 'react-bootstrap';
import Header from './Header.js'
import InputForm from './InputForm.js'
import OutputForm from './OutputForm.js'

export function Main() {
    const [validated, setValidated] = useState(false);

    const handleSubmit = (event) => {
      const form = event.currentTarget;
      if (form.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
      }
  
      setValidated(true);
    };

    return (
        <>
            <Container>
                <Row></Row>
                <Row>
                 <Header/> 
                 </Row>
                 <Row> 
                <Stack gap={4}>
                      
                    
                    <Form noValidate validated={validated} onSubmit={handleSubmit}>                                 
                        <InputForm/>
                    </Form>            
                
                    <Stack direction="horizontal" gap={3}>
                        <Button type="submit" variant="outline-primary">Convert</Button>   
                        <Button type="button" variant="outline-secondary" disabled>Cancel</Button>   
                    </Stack>            
                
                    <OutputForm/>                
                </Stack>   </Row>
            </Container>       
        </>
    );
}