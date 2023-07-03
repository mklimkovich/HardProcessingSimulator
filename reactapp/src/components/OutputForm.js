import { Form, Spinner } from 'react-bootstrap';

export default function OutputForm () {
    return (
        <>    
         
         <Form.Control as="textarea" rows={10} disabled/>

<Spinner
    as="span"
    animation="border"
    size="sm"
    role="status"
    aria-hidden="true"
    />
    <span className="visually-hidden">Loading...</span>
        
                        
        </>
    );
}