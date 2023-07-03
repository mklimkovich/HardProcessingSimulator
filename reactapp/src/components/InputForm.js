import { Form, FormGroup } from 'react-bootstrap';

export default function InputForm () {
    return (
        <>
            <FormGroup controlId="inputArea">
                <Form.Control as="textarea" rows={10} placeholder="Enter text here..." />
                <Form.Text id="validationError" muted></Form.Text>
            </FormGroup>
        </>
    );
}