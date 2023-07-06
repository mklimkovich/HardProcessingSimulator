import 'bootstrap/dist/css/bootstrap.min.css';
import './App.css';

import { Main } from './components/Main.js'

export default function App() {
    return (
        <>
            <Main serverUrl={process.env.REACT_APP_BACKEND_URL} />
        </>
    );
}
