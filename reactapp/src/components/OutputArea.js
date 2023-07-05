import { Form } from 'react-bootstrap';

export default function OutputArea ({ value }) {
    return (
        <>    
         <Form.Control value={value} as="textarea" rows={10} disabled />
        </>
    );
}
