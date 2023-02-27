import { Navbar, NavbarBrand } from "reactstrap"

export default function NavPanel() {
    return (
        <Navbar color="light">
            <NavbarBrand href="/" className="fw-bold text-secondary fs-3 mx-auto">
                GGoose Blog
            </NavbarBrand>
        </Navbar>
    );
}