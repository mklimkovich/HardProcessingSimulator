import { Form, Spinner } from 'react-bootstrap';

export default function OutputArea ({ value, enabled }) {
    return (
        <>    
        <div style={{ "position": "relative" }}>
         <Form.Control value={value} as="textarea" rows={10} disabled style={{ "position": "absolute" }} />
        

{/* <Spinner animation="border" role="status"
style={{ "display": "block", "position": "initial", "z-index": 9, "top": "50%" }}
 className={enabled ? "modal show" : "visually-hidden"}>
      <span className="visually-hidden">Loading...</span>
    </Spinner> */}
        
    </div>
        </>
    );
}
