import './App.css';
import Navbar from './components/Navbar/Navbar';
import AllRoutes from './components/Routes/AllRoutes';
import Footer from './components/Footer/Footer';
import BottomNav from './components/BottomNavigation/BottomNav';
import LocationMap from './components/Localtionmap/Locationmap';

function App() {
    return (
        <div>
            <Navbar />
            <AllRoutes />
            <Footer />
            <BottomNav />
        </div>
    );
}

export default App;
