import { useState } from 'react';
import { Form, FormGroup } from 'react-bootstrap';

export default function InputForm({ formId, onValidationSucceed, disabled }) {
    const [value, setValue] = useState('');
    const [validated, setValidated] = useState(false);

    const onInput = (e) => setValue(e.target.value);

    const onHandleSubmit = (event) => {
      const form = event.currentTarget;

      event.stopPropagation();
      event.preventDefault();

      if (form.checkValidity() === true) {
        onValidationSucceed(value);
        setValidated(undefined);
      }    
      else
      {
        setValidated(true);
      }
      
      setValue(value);
    }

    return (
        <>
            <Form id={formId} noValidate validated={validated} onSubmit={onHandleSubmit}>                                 
                <FormGroup>
                    <Form.Control value={value} onInput={onInput}
                                  disabled={disabled} as="textarea" rows={10}
                                  placeholder="Enter text here..." required />
                    <Form.Control.Feedback type="invalid">
                        Please provide text for encoding.
                    </Form.Control.Feedback>
                </FormGroup>
            </Form>   
        </>
    );
}